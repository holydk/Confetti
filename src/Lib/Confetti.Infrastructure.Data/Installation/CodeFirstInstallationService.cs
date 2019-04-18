using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Common.Data;
using Confetti.Common.Installation;
using Confetti.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Data.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;
        private readonly IRepository<SpecificationAttributeOption> _specificationAttributeOptionRepository;
        private readonly IRepository<ProductSpecificationAttribute> _productSpecificationAttributeRepository;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(
            IRepository<Category> categoryRepository,
            IRepository<Product> productRepository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<SpecificationAttribute> specificationAttributeRepository,
            IRepository<SpecificationAttributeOption> specificationAttributeOptionRepository,
            IRepository<ProductSpecificationAttribute> productSpecificationAttributeRepository
        )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _specificationAttributeRepository = specificationAttributeRepository;
            _specificationAttributeOptionRepository = specificationAttributeOptionRepository;
            _productSpecificationAttributeRepository = productSpecificationAttributeRepository;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Install data
        /// </summary>
        /// <param name="updateData">A value indicating whether to rewrite old data</param>
        public async Task InstallDataAsync(bool updateData)
        {
            if (updateData)
            {
                var categories = await _categoryRepository.Table.ToListAsync();
                await _categoryRepository.DeleteAsync(categories);
                var psams = await _productSpecificationAttributeRepository.Table.ToListAsync();
                await _productSpecificationAttributeRepository.DeleteAsync(psams);
                var saos = await _specificationAttributeOptionRepository.Table.ToListAsync();
                await _specificationAttributeOptionRepository.DeleteAsync(saos);
                var sas = await _specificationAttributeRepository.Table.ToListAsync();
                await _specificationAttributeRepository.DeleteAsync(sas);
                var pcs = await _productCategoryRepository.Table.ToListAsync();
                await _productCategoryRepository.DeleteAsync(pcs);
                var ps = await _productRepository.Table.ToListAsync();
                await _productRepository.DeleteAsync(ps);
            }

            if (!_categoryRepository.Table.Any())
            {
                // Categories
                var allCategories = new List<Category>();
                var c1 = new Category()
                {
                    Title = "Детям",
                    MetaTitle = "Купить детскую одежду и обувь | Confetti",
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1);
                await _categoryRepository.InsertAsync(c1);
                
                var c1Sub1 = new Category()
                {
                    Title = "Новинки",
                    MetaTitle = "Новинки детской одежды и обуви | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub1);
                await _categoryRepository.InsertAsync(c1Sub1);

                var c1Sub2 = new Category()
                {
                    Title = "Девочкам",
                    MetaTitle = "Купить одежду обувь и аксессуары для девочек | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub2);
                await _categoryRepository.InsertAsync(c1Sub2);

                var c1Sub3 = new Category()
                {
                    Title = "Мальчикам",
                    MetaTitle = "Купить одежду обувь и аксессуары для мальчиков | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub3);
                await _categoryRepository.InsertAsync(c1Sub3);

                var c1Sub4 = new Category()
                {
                    Title = "Новорожденным",
                    MetaTitle = "Купить одежду и обувь для новорожденных | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub4);
                await _categoryRepository.InsertAsync(c1Sub4);

                var c1Sub5 = new Category()
                {
                    Title = "Спорт",
                    MetaTitle = "Купить одежду и обувь для физкультуры и спорта | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub5);
                await _categoryRepository.InsertAsync(c1Sub5);

                var c1Sub6 = new Category()
                {
                    Title = "Школа",
                    MetaTitle = "Купить школьную одежду и обувь | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c1Sub6);
                await _categoryRepository.InsertAsync(c1Sub6);

                var c1Sub7 = new Category()
                {
                    Title = "Premium",
                    MetaTitle = "Купить эксклюзивную одежду и обувь для детей | Confetti",
                    ParentId = c1.Id,
                    IsIncludeInTopMenu = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = true
                };
                allCategories.Add(c1Sub7);
                await _categoryRepository.InsertAsync(c1Sub7);

                var c2 = new Category()
                {
                    Title = "Мужчинам",
                    MetaTitle = "Купить мужскую одежду и обувь | Confetti",
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2);
                await _categoryRepository.InsertAsync(c2);

                var c2Sub1 = new Category()
                {
                    Title = "Новинки",
                    MetaTitle = "Новинки мужской одежды и обуви | Confetti",
                    ParentId = c2.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2Sub1);
                await _categoryRepository.InsertAsync(c2Sub1);

                var c2Sub2 = new Category()
                {
                    Title = "Одежда",
                    MetaTitle = "Купить мужскую одежду | Confetti",
                    ParentId = c2.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2Sub2);
                await _categoryRepository.InsertAsync(c2Sub2);

                var c2Sub2Sub1 = new Category()
                {
                    Title = "Джинсы",
                    MetaTitle = "Купить мужские джинсы | Confetti",
                    ParentId = c2Sub2.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2Sub2Sub1);
                await _categoryRepository.InsertAsync(c2Sub2Sub1);

                var c2Sub3 = new Category()
                {
                    Title = "Обувь",
                    MetaTitle = "Купить мужскую обувь | Confetti",
                    ParentId = c2.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2Sub3);
                await _categoryRepository.InsertAsync(c2Sub3);

                var c2Sub4 = new Category()
                {
                    Title = "Спорт",
                    MetaTitle = "Купить спортивную мужскую одежду и обувь | Confetti",
                    ParentId = c2.Id,
                    IsIncludeInTopMenu = true,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = false
                };
                allCategories.Add(c2Sub4);
                await _categoryRepository.InsertAsync(c2Sub4);

                var c2Sub5 = new Category()
                {
                    Title = "Подарки",
                    MetaTitle = "Купить подарки для мужчин | Confetti",
                    ParentId = c2.Id,
                    IsIncludeInTopMenu = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = true
                };
                allCategories.Add(c2Sub5);
                await _categoryRepository.InsertAsync(c2Sub5);

                var c2Sub5Sub1 = new Category()
                {
                    Title = "Эксклюзивные",
                    MetaTitle = "Купить эксклюзивные подарки для мужчин | Confetti",
                    ParentId = c2Sub5.Id,
                    IsIncludeInTopMenu = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow,
                    Position = 0,
                    Deleted = false,
                    Published = true,
                    ShowOnHomePage = true
                };
                allCategories.Add(c2Sub5Sub1);
                await _categoryRepository.InsertAsync(c2Sub5Sub1);

                // Джинсы c2Sub2Sub1
                var p1_c2Sub2Sub1 = new Product()
                {
                    Name = "Mango Man",
                    ShowOnHomePage = false,
                    MetaTitle = "Купить джинсы Mango Man | Confetti",
                    IsGiftCard = false,
                    GiftCardTypeId = 0,
                    IsShipEnabled = true,
                    IsFreeShipping = false,
                    ShipSeparately = false,
                    AdditionalShippingCharge = 0,
                    DeliveryDateId = 0,
                    ManageInventoryMethodId = 0,
                    ProductAvailabilityRangeId = 0,
                    UseMultipleWarehouses = false,
                    WarehouseId = 0,
                    StockQuantity = 10000,
                    DisplayStockAvailability = false,
                    MinStockQuantity = 1,
                    LowStockActivityId = 0,
                    NotifyAdminForQuantityBelow = 1,
                    BackorderModeId = 0,
                    AllowBackInStockSubscriptions = true,
                    OrderMinimumQuantity = 1,
                    OrderMaximumQuantity = 10,
                    NotReturnable = false,
                    DisableBuyButton = false,
                    DisableWishlistButton = false,
                    AvailableForPreOrder = false,
                    Price = 2999,
                    OldPrice = 2999,
                    ProductCost = 1500,
                    MarkAsNew = false,
                    HasDiscountsApplied = false,
                    Weight = 10,
                    Length = 10,
                    Height = 10,
                    Position = 0,
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                await _productRepository.InsertAsync(p1_c2Sub2Sub1);
                var p1_c2Sub2Sub1_map = new ProductCategory()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    CategoryId = c2Sub2Sub1.Id,
                    IsFeaturedProduct = false,
                    Position = 0
                };
                await _productCategoryRepository.InsertAsync(p1_c2Sub2Sub1_map);

                var p2_c2Sub2Sub1 = new Product()
                {
                    Name = "Burton Menswear London",
                    ShowOnHomePage = false,
                    MetaTitle = "Купить джинсы Burton Menswear London | Confetti",
                    IsGiftCard = false,
                    GiftCardTypeId = 0,
                    IsShipEnabled = true,
                    IsFreeShipping = false,
                    ShipSeparately = false,
                    AdditionalShippingCharge = 0,
                    DeliveryDateId = 0,
                    ManageInventoryMethodId = 0,
                    ProductAvailabilityRangeId = 0,
                    UseMultipleWarehouses = false,
                    WarehouseId = 0,
                    StockQuantity = 10000,
                    DisplayStockAvailability = false,
                    MinStockQuantity = 1,
                    LowStockActivityId = 0,
                    NotifyAdminForQuantityBelow = 1,
                    BackorderModeId = 0,
                    AllowBackInStockSubscriptions = true,
                    OrderMinimumQuantity = 1,
                    OrderMaximumQuantity = 10,
                    NotReturnable = false,
                    DisableBuyButton = false,
                    DisableWishlistButton = false,
                    AvailableForPreOrder = false,
                    Price = 2899,
                    OldPrice = 2999,
                    ProductCost = 1500,
                    MarkAsNew = false,
                    HasDiscountsApplied = false,
                    Weight = 10,
                    Length = 10,
                    Height = 10,
                    Position = 0,
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                await _productRepository.InsertAsync(p2_c2Sub2Sub1);
                var p2_c2Sub2Sub1_map = new ProductCategory()
                {
                    ProductId = p2_c2Sub2Sub1.Id,
                    CategoryId = c2Sub2Sub1.Id,
                    IsFeaturedProduct = false,
                    Position = 0
                };
                await _productCategoryRepository.InsertAsync(p2_c2Sub2Sub1_map);

                var p3_c2Sub2Sub1 = new Product()
                {
                    Name = "Boss Hugo Boss",
                    ShowOnHomePage = false,
                    MetaTitle = "Купить джинсы Boss Hugo Boss | Confetti",
                    IsGiftCard = false,
                    GiftCardTypeId = 0,
                    IsShipEnabled = true,
                    IsFreeShipping = false,
                    ShipSeparately = false,
                    AdditionalShippingCharge = 0,
                    DeliveryDateId = 0,
                    ManageInventoryMethodId = 0,
                    ProductAvailabilityRangeId = 0,
                    UseMultipleWarehouses = false,
                    WarehouseId = 0,
                    StockQuantity = 10000,
                    DisplayStockAvailability = false,
                    MinStockQuantity = 1,
                    LowStockActivityId = 0,
                    NotifyAdminForQuantityBelow = 1,
                    BackorderModeId = 0,
                    AllowBackInStockSubscriptions = true,
                    OrderMinimumQuantity = 1,
                    OrderMaximumQuantity = 10,
                    NotReturnable = false,
                    DisableBuyButton = false,
                    DisableWishlistButton = false,
                    AvailableForPreOrder = false,
                    Price = 14500,
                    OldPrice = 14000,
                    ProductCost = 10000,
                    MarkAsNew = false,
                    HasDiscountsApplied = false,
                    Weight = 10,
                    Length = 10,
                    Height = 10,
                    Position = 0,
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                await _productRepository.InsertAsync(p3_c2Sub2Sub1);
                var p3_c2Sub2Sub1_map = new ProductCategory()
                {
                    ProductId = p3_c2Sub2Sub1.Id,
                    CategoryId = c2Sub2Sub1.Id,
                    IsFeaturedProduct = false,
                    Position = 0
                };
                await _productCategoryRepository.InsertAsync(p3_c2Sub2Sub1_map);

                var p4_c2Sub2Sub1 = new Product()
                {
                    Name = "Levi's®",
                    ShowOnHomePage = false,
                    MetaTitle = "Купить джинсы Levi's® | Confetti",
                    IsGiftCard = false,
                    GiftCardTypeId = 0,
                    IsShipEnabled = true,
                    IsFreeShipping = false,
                    ShipSeparately = false,
                    AdditionalShippingCharge = 0,
                    DeliveryDateId = 0,
                    ManageInventoryMethodId = 0,
                    ProductAvailabilityRangeId = 0,
                    UseMultipleWarehouses = false,
                    WarehouseId = 0,
                    StockQuantity = 10000,
                    DisplayStockAvailability = false,
                    MinStockQuantity = 1,
                    LowStockActivityId = 0,
                    NotifyAdminForQuantityBelow = 1,
                    BackorderModeId = 0,
                    AllowBackInStockSubscriptions = true,
                    OrderMinimumQuantity = 1,
                    OrderMaximumQuantity = 10,
                    NotReturnable = false,
                    DisableBuyButton = false,
                    DisableWishlistButton = false,
                    AvailableForPreOrder = false,
                    Price = 7500,
                    OldPrice = 6899,
                    ProductCost = 4899,
                    MarkAsNew = false,
                    HasDiscountsApplied = false,
                    Weight = 10,
                    Length = 10,
                    Height = 10,
                    Position = 0,
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                await _productRepository.InsertAsync(p4_c2Sub2Sub1);
                var p4_c2Sub2Sub1_map = new ProductCategory()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    CategoryId = c2Sub2Sub1.Id,
                    IsFeaturedProduct = false,
                    Position = 0
                };
                await _productCategoryRepository.InsertAsync(p4_c2Sub2Sub1_map);

                var p5_c2Sub2Sub1 = new Product()
                {
                    Name = "Modis",
                    ShowOnHomePage = false,
                    MetaTitle = "Купить джинсы Modis | Confetti",
                    IsGiftCard = false,
                    GiftCardTypeId = 0,
                    IsShipEnabled = true,
                    IsFreeShipping = false,
                    ShipSeparately = false,
                    AdditionalShippingCharge = 0,
                    DeliveryDateId = 0,
                    ManageInventoryMethodId = 0,
                    ProductAvailabilityRangeId = 0,
                    UseMultipleWarehouses = false,
                    WarehouseId = 0,
                    StockQuantity = 10000,
                    DisplayStockAvailability = false,
                    MinStockQuantity = 1,
                    LowStockActivityId = 0,
                    NotifyAdminForQuantityBelow = 1,
                    BackorderModeId = 0,
                    AllowBackInStockSubscriptions = true,
                    OrderMinimumQuantity = 1,
                    OrderMaximumQuantity = 10,
                    NotReturnable = false,
                    DisableBuyButton = false,
                    DisableWishlistButton = false,
                    AvailableForPreOrder = false,
                    Price = 1500,
                    OldPrice = 1500,
                    ProductCost = 800,
                    MarkAsNew = false,
                    HasDiscountsApplied = false,
                    Weight = 10,
                    Length = 10,
                    Height = 10,
                    Position = 0,
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                await _productRepository.InsertAsync(p5_c2Sub2Sub1);
                var p5_c2Sub2Sub1_map = new ProductCategory()
                {
                    ProductId = p5_c2Sub2Sub1.Id,
                    CategoryId = c2Sub2Sub1.Id,
                    IsFeaturedProduct = false,
                    Position = 0
                };
                await _productCategoryRepository.InsertAsync(p5_c2Sub2Sub1_map);

                // Specification attributes
                var sa1 = new SpecificationAttribute()
                {
                    Name = "Цвет",
                    Position = 1
                };
                await _specificationAttributeRepository.InsertAsync(sa1);
                var sa2 = new SpecificationAttribute()
                {
                    Name = "Размер",
                    Position = 2
                };
                await _specificationAttributeRepository.InsertAsync(sa2);
                var sa3 = new SpecificationAttribute()
                {
                    Name = "Материал",
                    Position = 3
                };
                await _specificationAttributeRepository.InsertAsync(sa3);
                var sa4 = new SpecificationAttribute()
                {
                    Name = "Узор",
                    Position = 4
                };
                await _specificationAttributeRepository.InsertAsync(sa4);

                // Specification attribute options
                // Цвет sa1
                var sa1_sao1 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa1.Id,
                    Name = "Белый"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa1_sao1);
                var sa1_sao2 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa1.Id,
                    Name = "Черный"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa1_sao2);
                var sa1_sao3 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa1.Id,
                    Name = "Красный"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa1_sao3);
                var sa1_sao4 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa1.Id,
                    Name = "Серый"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa1_sao4);

                // Размер sa2
                var sa2_sao1 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa2.Id,
                    Name = "28"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa2_sao1);
                var sa2_sao2 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa2.Id,
                    Name = "29"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa2_sao2);
                var sa2_sao3 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa2.Id,
                    Name = "30"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa2_sao3);
                var sa2_sao4 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa2.Id,
                    Name = "31"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa2_sao4);

                // Материал sa3
                var sa3_sao1 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa3.Id,
                    Name = "Вискоза"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa3_sao1);
                var sa3_sao2 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa3.Id,
                    Name = "Лайкра"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa3_sao2);
                var sa3_sao3 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa3.Id,
                    Name = "Лен"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa3_sao3);
                var sa3_sao4 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa3.Id,
                    Name = "Полиэстер"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa3_sao4);
                var sa3_sao5 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa3.Id,
                    Name = "Хлопок"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa3_sao5);

                // Узор sa4
                var sa4_sao1 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa4.Id,
                    Name = "Одноптонный"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa4_sao1);
                var sa4_sao2 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa4.Id,
                    Name = "Клетка"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa4_sao2);
                var sa4_sao3 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa4.Id,
                    Name = "Полоска"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa4_sao3);
                var sa4_sao4 = new SpecificationAttributeOption()
                {
                    SpecificationAttributeId = sa4.Id,
                    Name = "Другое"
                };
                await _specificationAttributeOptionRepository.InsertAsync(sa4_sao4);

                // Product_SpecificationAttribute_Mapping
                
                // Mango Man p1_c2Sub2Sub1
                // Белый Цвет sa1_sao1
                var p1_sa1_sao1_psam1 = new ProductSpecificationAttribute()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa1_sao1.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p1_sa1_sao1_psam1);
                // Размер 28 sa2_sao1
                var p1_sa2_sao1_psam2 = new ProductSpecificationAttribute()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao1.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p1_sa2_sao1_psam2);
                // Размер 29 sa2_sao2
                var p1_sa2_sao2_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao2.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p1_sa2_sao2_psam3);
                // Размер 30 sa2_sao3
                var p1_sa2_sao3_psam4 = new ProductSpecificationAttribute()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao3.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p1_sa2_sao3_psam4);
                // Узор однотонный sa4_sao1
                var p1_sa4_sao1_psam5 = new ProductSpecificationAttribute()
                {
                    ProductId = p1_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa4_sao1.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p1_sa4_sao1_psam5);

                // Burton Menswear London p2_c2Sub2Sub1
                // Белый Цвет sa1_sao1
                var p2_sa1_sao1_psam1 = new ProductSpecificationAttribute()
                {
                    ProductId = p2_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa1_sao1.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p2_sa1_sao1_psam1);
                // Размер 30 sa2_sao3
                var p2_sa2_sao3_psam2 = new ProductSpecificationAttribute()
                {
                    ProductId = p2_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao3.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p2_sa2_sao3_psam2);
                // Размер 31 sa2_sao4
                var p2_sa2_sao4_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p2_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao4.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p2_sa2_sao4_psam3);

                // Boss Hugo Boss p3_c2Sub2Sub1
                // Черный Цвет sa1_sao2
                var p3_sa1_sao2_psam1 = new ProductSpecificationAttribute()
                {
                    ProductId = p3_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa1_sao2.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p3_sa1_sao2_psam1);
                // Материал Хлопок sa3_sao5
                var p3_sa3_sao5_psam2 = new ProductSpecificationAttribute()
                {
                    ProductId = p3_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa3_sao5.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p3_sa3_sao5_psam2);
                // Размер 29 sa2_sao2
                var p3_sa2_sao2_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p3_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao2.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p3_sa2_sao2_psam3);

                // Levi's® p4_c2Sub2Sub1
                // Черный Цвет sa1_sao2
                var p4_sa1_sao2_psam1 = new ProductSpecificationAttribute()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa1_sao2.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p4_sa1_sao2_psam1);
                // Серый Цвет sa1_sao4
                var p4_sa1_sao4_psam2 = new ProductSpecificationAttribute()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa1_sao4.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p4_sa1_sao4_psam2);
                // Размер 28 sa2_sao1
                var p4_sa2_sao1_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa2_sao1.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p4_sa2_sao1_psam3);
                // Материал Полиэстер sa3_sao4
                var p4_sa3_sao4_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa3_sao4.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p4_sa3_sao4_psam3);
                // Узор Другое sa4_sao4
                var p4_sa4_sao4_psam3 = new ProductSpecificationAttribute()
                {
                    ProductId = p4_c2Sub2Sub1.Id,
                    AttributeTypeId = 0,
                    SpecificationAttributeOptionId = sa4_sao4.Id,
                    ShowOnProductPage = false,
                    AllowFiltering = true
                };
                await _productSpecificationAttributeRepository.InsertAsync(p4_sa4_sao4_psam3);
            }
        }
            
        #endregion
    }
}