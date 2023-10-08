using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Services;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class CategoryServiceFixture : BaseFixture
    {
        private CategoryService categoryService = null!;

        [TestInitialize]
        public void Initialize()
        {
            DataContext context = GetInMemoryDatabaseContext();
            categoryService = new CategoryService(context);
        }

        [TestMethod]
        public void ShouldCreateCategory()
        {
            var category = new CategoryDb
            {
                Name = "New Category",
                Description = "New Category Description",
            };

            var createdCategory = categoryService.Create(category);

            var result = categoryService.GetById(createdCategory.CategoryId);

            Assert.IsNotNull(createdCategory);
            Assert.AreEqual(category.Name, result.Name);
            Assert.AreEqual(category.Description, result.Description);
        }

        [TestMethod]
        public void ShouldUpdateCategory()
        {
            var category = new CategoryDb
            {
                Name = "New Category",
                Description = "New Category Description",
            };
            var createdCategory = categoryService.Create(category);

            createdCategory.Name = "Updated Category";
            createdCategory.Description = "Updated Description";
            var updatedCategory = categoryService.Update(createdCategory);

            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Category", updatedCategory.Name);
            Assert.AreEqual("Updated Description", updatedCategory.Description);
        }

        [TestMethod]
        public void ShouldDeleteCategory()
        {
            var category = new CategoryDb
            {
                Name = "New Category",
                Description = "New Category Description",
            };

            var createdCategory = categoryService.Create(category);

            categoryService.Delete(createdCategory.CategoryId);
            var deletedCategory = categoryService.GetById(createdCategory.CategoryId);

            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        public void ShouldReturnAllCategories()
        {
            var result = categoryService.GetAll();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}