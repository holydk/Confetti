using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Domain.Catalog;
using Confetti.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Application.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<Category> _categoryRepository;
            
        #endregion

        #region Ctor

        public CodeFirstInstallationService(
            IRepository<Category> categoryRepository
        )
        {
            _categoryRepository = categoryRepository;
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
            }
        }
            
        #endregion
    }
}