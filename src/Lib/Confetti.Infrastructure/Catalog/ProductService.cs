using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Confetti.Application.Catalog;
using Confetti.Application.Models.Catalog;
using Confetti.Common.Data;
using Confetti.Domain.Catalog;
using Confetti.Infrastructure.Collections;
using Confetti.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial class ProductService : IProductService
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly DbContext _context;
        private readonly IDataProvider _dataProvider;
        private readonly IRepository<Product> _productRepository;
            
        #endregion

        #region Ctor
        
        public ProductService(
            IMapper mapper,
            DbContext context,
            IDataProvider dataProvider,
            IRepository<Product> productRepository
        )
        {
            _mapper = mapper;
            _context = context;
            _dataProvider = dataProvider;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods
            
        #region Products

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="loadFilterableCountableSpecificationAttributeOptionIds">a value indicating whether we should load the specification attribute option identifiers applied to loaded products (all pages) with count of products</param>
        /// <param name="loadMinMaxPrices">a value indicating whether we should load min and max prices (all pages)</param>
        /// <param name="searchModel">search product model</param>
        /// <returns>Products</returns>
        public async Task<ProductsSearchModelResult> SearchProductsAsync(
            bool loadFilterableCountableSpecificationAttributeOptionIds = false,
            bool loadMinMaxPrices = false,
            ProductsSearchModel searchModel = null
        )
        {
            if (searchModel == null)
                searchModel = new ProductsSearchModel();

            // validate "CategoryIds"
            if (searchModel.CategoryIds != null && searchModel.CategoryIds.Contains(0))
                searchModel.CategoryIds.Remove(0);

            // pass category identifiers as comma-delimited string
            var commaSeparatedCategoryIds = searchModel.CategoryIds == null 
                ? string.Empty 
                : string.Join(",", searchModel.CategoryIds);

            // pass specification identifiers as comma-delimited string
            var commaSeparatedSpecIds = string.Empty;
            if (searchModel.FilteredSpecs != null)
            {
                ((List<int>)searchModel.FilteredSpecs).Sort();
                commaSeparatedSpecIds = string.Join(",", searchModel.FilteredSpecs);
            }

            // some databases don't support int.MaxValue
            if (searchModel.PageSize == int.MaxValue)
                searchModel.PageSize = int.MaxValue - 1;
                
            var pCategoryIds = _dataProvider.GetStringParameter("CategoryIds", commaSeparatedCategoryIds);
            var pWarehouseId = _dataProvider.GetInt32Parameter("WarehouseId", searchModel.WarehouseId);
            var pMarkedAsNewOnly = _dataProvider.GetBooleanParameter("MarkedAsNewOnly", searchModel.MarkedAsNewOnly);
            var pFeaturedProducts = _dataProvider.GetBooleanParameter("FeaturedProducts", searchModel.FeaturedProducts);
            var pPriceMin = _dataProvider.GetDecimalParameter("PriceMin", searchModel.PriceMin);
            var pPriceMax = _dataProvider.GetDecimalParameter("PriceMax", searchModel.PriceMax);
            var pKeywords = _dataProvider.GetStringParameter("Keywords", searchModel.Keywords);
            var pSearchDescriptions = _dataProvider.GetBooleanParameter("SearchDescriptions", searchModel.SearchDescriptions);
            var pSearchSku = _dataProvider.GetBooleanParameter("SearchSku", searchModel.SearchSku);
            var pFilteredSpecs = _dataProvider.GetStringParameter("FilteredSpecs", commaSeparatedSpecIds);
            var pOrderBy = _dataProvider.GetInt32Parameter("OrderBy", (int)searchModel.OrderBy);
            var pPageIndex = _dataProvider.GetInt32Parameter("PageIndex", searchModel.PageIndex);
            var pPageSize = _dataProvider.GetInt32Parameter("PageSize", searchModel.PageSize);
            var pShowHidden = _dataProvider.GetBooleanParameter("ShowHidden", searchModel.ShowHidden);
            var pOverridePublished = _dataProvider.GetBooleanParameter("OverridePublished", searchModel.OverridePublished);
            var pLoadFilterableCountableSpecificationAttributeOptionIds = _dataProvider.GetBooleanParameter(
                "LoadFilterableCountableSpecificationAttributeOptionIds", loadFilterableCountableSpecificationAttributeOptionIds);
            var pLoadMinMaxPrices = _dataProvider.GetBooleanParameter("LoadMinMaxPrices", loadMinMaxPrices);

            // prepare output parameters
            var pFilterableCountableSpecificationAttributeOptionIds = _dataProvider
                .GetOutputStringParameter("FilterableCountableSpecificationAttributeOptionIds");
            pFilterableCountableSpecificationAttributeOptionIds.Size = int.MaxValue - 1;
            var pTotalRecords = _dataProvider.GetOutputInt32Parameter("TotalRecords");
            var pMinPrice = _dataProvider.GetOutputDecimalParameter("MinPrice");
            var pMaxPrice = _dataProvider.GetOutputDecimalParameter("MaxPrice");

            // invoke stored procedure
            var products = await _context.EntityFromSql<Product>("ProductLoadAllPaged",
                pCategoryIds,
                pWarehouseId,
                pMarkedAsNewOnly,
                pFeaturedProducts,
                pPriceMin,
                pPriceMax,
                pKeywords,
                pSearchDescriptions,
                pSearchSku,
                pFilteredSpecs,
                pOrderBy,
                pPageIndex,
                pPageSize,
                pShowHidden,
                pOverridePublished,
                pLoadFilterableCountableSpecificationAttributeOptionIds,
                pFilterableCountableSpecificationAttributeOptionIds,
                pLoadMinMaxPrices,
                pMinPrice,
                pMaxPrice,
                pTotalRecords)
                .ProjectTo<ProductModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var totalRecords = pTotalRecords.Value != DBNull.Value 
                ? Convert.ToInt32(pTotalRecords.Value) 
                : 0;

            var result = new ProductsSearchModelResult()
            {
                Products = new PagedList<ProductModel>(
                    products, 
                    searchModel.PageIndex, 
                    searchModel.PageSize, 
                    totalRecords)
            };

            // get filterable specification attribute option identifiers
            var filterableCountableSpecificationAttributeOptionIdsStr =
                pFilterableCountableSpecificationAttributeOptionIds.Value != DBNull.Value
                    ? (string)pFilterableCountableSpecificationAttributeOptionIds.Value
                    : string.Empty;

            if (loadFilterableCountableSpecificationAttributeOptionIds &&
                !string.IsNullOrWhiteSpace(filterableCountableSpecificationAttributeOptionIdsStr))
            {
                result.CountableSpecificationAttributeOptionIds = filterableCountableSpecificationAttributeOptionIdsStr.Split(
                    new[] { ',' }, 
                    StringSplitOptions.RemoveEmptyEntries
                )
                .Select(countableSpecsOptionStr => 
                {
                    var countableSpecsOption = countableSpecsOptionStr.Split(
                        new[] { '-' }, 
                        StringSplitOptions.RemoveEmptyEntries
                    );
                    
                    return new CountableSpecificationAttributeOption()
                    {
                        SpecificationAttributeOptionId = Convert.ToInt32(countableSpecsOption[0].Trim()),
                        CountOfProducts = Convert.ToInt32(countableSpecsOption[1].Trim())
                    };
                })
                .ToList();
            }

            if (loadMinMaxPrices)
            {
                result.MinPrice = pMinPrice.Value != DBNull.Value
                    ? (decimal)pMinPrice.Value
                    : 0;
                result.MaxPrice = pMaxPrice.Value != DBNull.Value
                    ? (decimal)pMaxPrice.Value
                    : 0;
            }

            return result;
        }
            
        #endregion

        #endregion
    }
}