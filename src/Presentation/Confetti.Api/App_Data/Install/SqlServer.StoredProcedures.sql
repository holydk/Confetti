IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[confetti_splitstring_to_table]') AND type in (N'TF'))
DROP FUNCTION [confetti_splitstring_to_table]
GO
CREATE FUNCTION [dbo].[confetti_splitstring_to_table]
(
    @string NVARCHAR(MAX),
    @delimiter CHAR(1)
)
RETURNS @output TABLE(
    data NVARCHAR(MAX)
)
BEGIN
    DECLARE @start INT, @end INT
    SELECT @start = 1, @end = CHARINDEX(@delimiter, @string)

    WHILE @start < LEN(@string) + 1 BEGIN
        IF @end = 0 
            SET @end = LEN(@string) + 1

        INSERT INTO @output (data) 
        VALUES(SUBSTRING(@string, @start, @end - @start))
        SET @start = @end + 1
        SET @end = CHARINDEX(@delimiter, @string, @start)
    END
    RETURN
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ProductLoadAllPaged]') AND type in (N'P', N'PC'))
DROP PROCEDURE [ProductLoadAllPaged]
GO
CREATE PROCEDURE [dbo].[ProductLoadAllPaged]
	@CategoryIds		nvarchar(MAX) = null,	--a list of category IDs (comma-separated list). e.g. 1,2,3
	@WarehouseId		int = 0,
	@MarkedAsNewOnly	bit = 0, 	--0 - load all products , 1 - "marked as new" only
	@FeaturedProducts	bit = null,	--0 featured only , 1 not featured only, null - load all products
	@PriceMin			decimal(18, 4) = null,
	@PriceMax			decimal(18, 4) = null,
	@Keywords			nvarchar(4000) = null,
	@SearchDescriptions bit = 0, --a value indicating whether to search by a specified "keyword" in product descriptions
	@SearchSku			bit = 0, --a value indicating whether to search by a specified "keyword" in product SKU
	@FilteredSpecs		nvarchar(MAX) = null,	--filter by specification attribute options (comma-separated list of IDs). e.g. 14,15,16
	@OrderBy			int = 0, --0 - position, 5 - Name: A to Z, 6 - Name: Z to A, 10 - Price: Low to High, 11 - Price: High to Low, 15 - creation date
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644,
	@ShowHidden			bit = 0,
	@OverridePublished	bit = null, --null - process "Published" property according to "showHidden" parameter, true - load only "Published" products, false - load only "Unpublished" products
	@LoadFilterableCountableSpecificationAttributeOptionIds bit = 0, --a value indicating whether we should load the specification attribute option identifiers applied to loaded products with count of products (all pages)
	@FilterableCountableSpecificationAttributeOptionIds nvarchar(MAX) = null OUTPUT, --the specification attribute option identifiers applied to loaded products (all pages) with count of products. returned as a comma separated list of identifiers
	@LoadMinMaxPrices	bit = 0, --a value indicating whether we should load min and max prices (all pages)
	@MinPrice			decimal(18, 4) = null OUTPUT, --min price (all pages)
	@MaxPrice			decimal(18, 4) = null OUTPUT, --max price (all pages)
	@TotalRecords		int = null OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    /* Products that filtered by keywords */
	CREATE TABLE #KeywordProducts
	(
		[ProductId] int NOT NULL
	)

	DECLARE
		@SearchKeywords bit,
		@OriginalKeywords nvarchar(4000),
		@sql nvarchar(max),
		@sql_orderby nvarchar(max)

	--filter by keywords
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = rtrim(ltrim(@Keywords))
	SET @OriginalKeywords = @Keywords

	IF ISNULL(@Keywords, '') != ''
	BEGIN

		SET @SearchKeywords = 1

		--usual search by PATINDEX
		SET @Keywords = '%' + @Keywords + '%'

		--product name
		SET @sql = '
			INSERT INTO #KeywordProducts (
				[ProductId]
			)
			SELECT 
				p.Id
			FROM 
				Product p with (NOLOCK)
			WHERE '
		SET @sql = @sql + 'PATINDEX(@Keywords, p.[Name]) > 0 '

		IF @SearchDescriptions = 1
		BEGIN

			--product short description
			SET @sql = @sql + '
				UNION
				SELECT 
					p.Id
				FROM 
					Product p with (NOLOCK)
				WHERE '
			SET @sql = @sql + 'PATINDEX(@Keywords, p.[ShortDescription]) > 0 '

			--product full description
			SET @sql = @sql + '
				UNION
				SELECT 
					p.Id
				FROM 
					Product p with (NOLOCK)
				WHERE '
			SET @sql = @sql + 'PATINDEX(@Keywords, p.[FullDescription]) > 0 '

		END

		--SKU (exact match)
		IF @SearchSku = 1
		BEGIN

			SET @sql = @sql + '
				UNION
				SELECT 
					p.Id
				FROM 
					Product p with (NOLOCK)
				WHERE 
					p.[Sku] = @OriginalKeywords '

		END

		--PRINT (@sql)
		EXEC sp_executesql @sql, N'@Keywords nvarchar(4000), @OriginalKeywords nvarchar(4000)', @Keywords, @OriginalKeywords

	END
	ELSE
	BEGIN
		SET @SearchKeywords = 0
	END
	
	--filter by category IDs
	SET @CategoryIds = isnull(@CategoryIds, '')	

	CREATE TABLE #FilteredCategoryIds
	(
		CategoryId int not null
	)

	INSERT INTO #FilteredCategoryIds (CategoryId)
	SELECT CAST(data as int) FROM [confetti_splitstring_to_table](@CategoryIds, ',')
	
	DECLARE @CategoryIdsCount int	
	SET @CategoryIdsCount = (SELECT COUNT(1) FROM #FilteredCategoryIds)	

	--paging
	DECLARE @PageLowerBound int
	DECLARE @PageUpperBound int
	DECLARE @RowsToReturn int
	SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
	SET @PageLowerBound = @PageSize * @PageIndex
	SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PositionTmp 
	(
		[Id] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int NOT NULL
	)

	SET @sql = '
		SELECT 
			p.Id
		FROM 
			Product p with (NOLOCK)'

	--searching by category IDs
	IF @CategoryIdsCount > 0
	BEGIN
		SET @sql = @sql + '
			INNER JOIN 
				Product_Category_Mapping pcm with (NOLOCK)
			ON 
				p.Id = pcm.ProductId'
	END

	--searching by keywords
	IF @SearchKeywords = 1
	BEGIN
		SET @sql = @sql + '
			JOIN 
				#KeywordProducts kp
			ON  
				p.Id = kp.ProductId'
	END

	SET @sql = @sql + '
		WHERE
			p.Deleted = 0'

	--filter by category
	IF @CategoryIdsCount > 0
	BEGIN

		SET @sql = @sql + '
			AND pcm.CategoryId IN ('
		
		SET @sql = @sql + + CAST(@CategoryIds AS nvarchar(max))

		SET @sql = @sql + ')'

		IF @FeaturedProducts IS NOT NULL
		BEGIN

			SET @sql = @sql + '
				AND pcm.IsFeaturedProduct = ' + CAST(@FeaturedProducts AS nvarchar(max))

		END
	END

	--filter by warehouse
	IF @WarehouseId > 0
	BEGIN

		--we should also ensure that 'ManageInventoryMethodId' is set to 'ManageStock' (1)
		--but we skip it in order to prevent hard-coded values (e.g. 1) and for better performance
		SET @sql = @sql + '
			AND  
			(
				(p.UseMultipleWarehouses = 0 
				 AND
				 p.WarehouseId = ' + CAST(@WarehouseId AS nvarchar(max)) + ')
				OR
				(p.UseMultipleWarehouses > 0 
				 AND
					EXISTS (
						SELECT 
							1 
						FROM 
							ProductWarehouseInventory [pwi]
						WHERE 
							[pwi].WarehouseId = ' + CAST(@WarehouseId AS nvarchar(max)) + ' AND [pwi].ProductId = p.Id
					)
				)
			)'

	END

	--filter by "marked as new"
	IF @MarkedAsNewOnly = 1
	BEGIN

		SET @sql = @sql + '
		AND 
			p.MarkAsNew = 1
		AND 
			(getutcdate() 
			 BETWEEN 
				ISNULL(p.MarkAsNewStartDateTimeUtc, ''1/1/1900'') 
				and 
				ISNULL(p.MarkAsNewEndDateTimeUtc, ''1/1/2999'')
			)'

	END

	--"Published" property
	IF (@OverridePublished is null)
	BEGIN

		--process according to "showHidden"
		IF @ShowHidden = 0
		BEGIN

			SET @sql = @sql + '			
				AND p.Published = 1'

		END
	END
	ELSE IF (@OverridePublished = 1)
	BEGIN

		--published only
		SET @sql = @sql + '
			AND p.Published = 1'

	END
	ELSE IF (@OverridePublished = 0)
	BEGIN

		--unpublished only
		SET @sql = @sql + '
			AND p.Published = 0'

	END

	--show hidden (AvailableDateTimeUtc Start/End)
	IF @ShowHidden = 0
	BEGIN

		SET @sql = @sql + '
			AND (
				getutcdate() 
				BETWEEN 
					ISNULL(p.AvailableStartDateTimeUtc, ''1/1/1900'') 
					and 
					ISNULL(p.AvailableEndDateTimeUtc, ''1/1/2999'')
			)'

	END

	--min price
	IF @PriceMin is not null
	BEGIN

		SET @sql = @sql + '
			AND (p.Price >= ' + CAST(@PriceMin AS nvarchar(max)) + ')'

	END
	
	--max price
	IF @PriceMax is not null
	BEGIN

		SET @sql = @sql + '
			AND (p.Price <= ' + CAST(@PriceMax AS nvarchar(max)) + ')'

	END

	--get specs for filtering
	CREATE TABLE #FilteredSpecs
	(
		SpecificationAttributeOptionId int not null
	)

	INSERT INTO 
		#FilteredSpecs (
			SpecificationAttributeOptionId
	)
	SELECT 
		CAST(data as int) 
	FROM 
		[confetti_splitstring_to_table](@FilteredSpecs, ',') 

	CREATE TABLE #FilteredSpecsWithAttributes
	(
		SpecificationAttributeId int not null,
		SpecificationAttributeOptionId int not null
	)

	INSERT INTO 
		#FilteredSpecsWithAttributes (
			SpecificationAttributeId, 
			SpecificationAttributeOptionId
	)
	SELECT 
		sao.SpecificationAttributeId, 
		fs.SpecificationAttributeOptionId
	FROM 
		#FilteredSpecs fs 
	INNER JOIN 
		SpecificationAttributeOption sao 
	ON 
		sao.Id = fs.SpecificationAttributeOptionId
	ORDER BY 
		sao.SpecificationAttributeId

	DECLARE @SpecAttributesCount int	
	SET @SpecAttributesCount = (SELECT COUNT(1) FROM #FilteredSpecsWithAttributes)

	--prepare available products (for performance)

	CREATE TABLE #AvailableProducts
	(
		[ProductId] int NOT NULL
	)

	IF ((@LoadFilterableCountableSpecificationAttributeOptionIds = 1
		OR
		@LoadMinMaxPrices = 1)
		AND 
		@SpecAttributesCount = 0)
	BEGIN

		SET @sql = '
			INSERT INTO 
				#AvailableProducts (
					[ProductId]
				) ' + @sql

		EXEC sp_executesql @sql

		SET @sql = '
			SELECT
				p.Id
			FROM 
				Product p WITH (NOLOCK)
			INNER JOIN 
				#AvailableProducts ap
			ON
				ap.ProductId = p.id'

	END

	--prepare filterable specification attribute option identifiers all products (if requested)
    IF (@LoadFilterableCountableSpecificationAttributeOptionIds = 1
		AND
		@SpecAttributesCount = 0)
	BEGIN

		CREATE TABLE #FilterableSpecs 
		(
			[SpecificationAttributeOptionId] int NOT NULL,
			[CountOfProducts] int NOT NULL
		)

        DECLARE @sql_filterableSpecs nvarchar(max)
		SET @sql_filterableSpecs = '
	        INSERT INTO 
				#FilterableSpecs (
					[SpecificationAttributeOptionId],
					[CountOfProducts]
				)
	        SELECT 
				[psam].SpecificationAttributeOptionId,
				COUNT([psam].ProductId)
	        FROM 
				[Product_SpecificationAttribute_Mapping] [psam] WITH (NOLOCK)
	        WHERE 
				[psam].[AllowFiltering] = 1
	        AND 
				[psam].[ProductId] IN (' + @sql + ')
			GROUP BY 
				[psam].SpecificationAttributeOptionId'

		EXEC sp_executesql @sql_filterableSpecs

		--build comma separated list of filterable identifiers
		SELECT 
			@FilterableCountableSpecificationAttributeOptionIds = 
				COALESCE(@FilterableCountableSpecificationAttributeOptionIds + ',' , '') 
				+ 
				CAST(SpecificationAttributeOptionId as nvarchar(4000))
				+
				'-'
				+
				CAST(CountOfProducts as nvarchar(4000))
		FROM 
			#FilterableSpecs

		DROP TABLE #FilterableSpecs

	END

	--prepare min/max prices all products (if requested)
	IF (@LoadMinMaxPrices = 1
		AND
		@SpecAttributesCount = 0)
	BEGIN

		CREATE TABLE #PriceRange 
		(
			[MinPrice] decimal(18, 4) NOT NULL,
			[MaxPrice] decimal(18, 4) NOT NULL
		)

		DECLARE @sql_minMaxPrices nvarchar(max)
		SET @sql_minMaxPrices = '
			INSERT INTO 
				#PriceRange (
					[MinPrice],
					[MaxPrice]
				)
			SELECT
				min(p_out.Price),
				max(p_out.Price)
			FROM
				Product p_out
			WHERE 
				p_out.Id IN (' + @sql + ')'

		EXEC sp_executesql @sql_minMaxPrices

		SELECT
			@MinPrice = #PriceRange.MinPrice,
			@MaxPrice = #PriceRange.MaxPrice
		FROM
			#PriceRange

		DROP TABLE #PriceRange

	END
	
	--filter by specification attribution options
	IF @SpecAttributesCount > 0
	BEGIN

		--do it for each specified specification option
		DECLARE @SpecificationAttributeOptionId int
		DECLARE @SpecificationAttributeId int
		DECLARE @LastSpecificationAttributeId int
        SET @LastSpecificationAttributeId = 0

		DECLARE cur_SpecificationAttributeOption CURSOR FOR
		SELECT 
			SpecificationAttributeId, 
			SpecificationAttributeOptionId
		FROM 
			#FilteredSpecsWithAttributes

		OPEN cur_SpecificationAttributeOption
        FOREACH:
            FETCH NEXT FROM 
				cur_SpecificationAttributeOption 
			INTO 
				@SpecificationAttributeId, 
				@SpecificationAttributeOptionId

            IF (@LastSpecificationAttributeId <> 0 
				AND 
				@SpecificationAttributeId <> @LastSpecificationAttributeId 
				OR 
				@@FETCH_STATUS <> 0)

			    SET @sql = @sql + '
					AND 
						p.Id IN (
							SELECT 
								psam.ProductId 
							FROM 
								[Product_SpecificationAttribute_Mapping] psam with (NOLOCK) 
							WHERE 
								psam.AllowFiltering = 1 
							AND 
								psam.SpecificationAttributeOptionId IN (
									SELECT 
										SpecificationAttributeOptionId 
									FROM 
										#FilteredSpecsWithAttributes 
									WHERE 
										SpecificationAttributeId = ' + CAST(@LastSpecificationAttributeId AS nvarchar(max)) + 
						'))'

            SET @LastSpecificationAttributeId = @SpecificationAttributeId

		IF @@FETCH_STATUS = 0 GOTO FOREACH

		CLOSE cur_SpecificationAttributeOption
		DEALLOCATE cur_SpecificationAttributeOption

	END

	--sorting
	SET @sql_orderby = ''	
	IF @OrderBy = 5 /* Name: A to Z */
		SET @sql_orderby = ' p.[Name] ASC'
	ELSE IF @OrderBy = 6 /* Name: Z to A */
		SET @sql_orderby = ' p.[Name] DESC'
	ELSE IF @OrderBy = 10 /* Price: Low to High */
		SET @sql_orderby = ' p.[Price] ASC'
	ELSE IF @OrderBy = 11 /* Price: High to Low */
		SET @sql_orderby = ' p.[Price] DESC'
	ELSE IF @OrderBy = 15 /* creation date */
		SET @sql_orderby = ' p.[CreatedOnUtc] DESC'
	ELSE /* default sorting, 0 (position) */
	BEGIN
		--category position
		IF @CategoryIdsCount > 0 SET @sql_orderby = ' pcm.Position ASC'
		
		--name
		IF LEN(@sql_orderby) > 0 SET @sql_orderby = @sql_orderby + ', '
		SET @sql_orderby = @sql_orderby + ' p.[Name] ASC'
	END

	SET @sql = @sql + '
		ORDER BY' + @sql_orderby

	SET @sql = '
		INSERT INTO #PositionTmp ([ProductId])' + @sql

	--PRINT (@sql)
	EXEC sp_executesql @sql

	--prepare available filterable specification attribute option identifiers by filtered specs (if requested)
    IF (@LoadFilterableCountableSpecificationAttributeOptionIds = 1
		AND
		@SpecAttributesCount > 0)
	BEGIN

		--build comma separated list of filterable identifiers
		SELECT
			@FilterableCountableSpecificationAttributeOptionIds = 
				COALESCE(@FilterableCountableSpecificationAttributeOptionIds + ',' , '') 
				+ 
				CAST([psam].SpecificationAttributeOptionId as nvarchar(4000))
				+
				'-'
				+
				CAST(COUNT([psam].ProductId) as nvarchar(4000))
		FROM 
			[Product_SpecificationAttribute_Mapping] [psam] WITH (NOLOCK)
		INNER JOIN 
			#PositionTmp p_tmp WITH (NOLOCK)
		ON 
			p_tmp.ProductId = [psam].ProductId
		WHERE 
			[psam].[AllowFiltering] = 1
		GROUP BY 
			[psam].SpecificationAttributeOptionId

	END

	--prepare min/max prices by filtered specs (if requested)
	IF (@LoadMinMaxPrices = 1
		AND
		@SpecAttributesCount > 0)
	BEGIN

		SELECT
			@MinPrice = MIN(p.Price),
			@MaxPrice = MAX(p.Price)
		FROM
			Product p
		INNER JOIN 
			#PositionTmp p_tmp WITH (NOLOCK)
		ON 
			p_tmp.ProductId = p.Id

	END

	DROP TABLE #FilteredCategoryIds
	DROP TABLE #FilteredSpecs
    DROP TABLE #FilteredSpecsWithAttributes
	DROP TABLE #KeywordProducts
	DROP TABLE #AvailableProducts

	CREATE TABLE #PageIndex 
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int NOT NULL
	)
	INSERT INTO #PageIndex (
		[ProductId]
	)
	SELECT 
		ProductId
	FROM 
		#PositionTmp
	GROUP BY 
		ProductId
	ORDER BY 
		min([Id])

	--total records
	SET @TotalRecords = @@rowcount
	
	DROP TABLE #PositionTmp

	--return products
	SELECT TOP (@RowsToReturn)
		p.*
	FROM
		#PageIndex [pi]
		INNER JOIN Product p with (NOLOCK) on p.Id = [pi].[ProductId]
	WHERE
		[pi].IndexId > @PageLowerBound AND 
		[pi].IndexId < @PageUpperBound
	ORDER BY
		[pi].IndexId
	
	DROP TABLE #PageIndex

END
GO
