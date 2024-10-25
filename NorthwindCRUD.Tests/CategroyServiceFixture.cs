using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class CategoryServiceFixture : BaseFixture
    {
        [TestMethod]
        public async Task ShouldCreateCategory()
        {
            var category = DataHelper.GetCategory();
            var createdCategory = await DataHelper.CategoryService.Create(category);

            Assert.IsNotNull(createdCategory);
            createdCategory = DataHelper2.CategoryService.GetById(createdCategory.CategoryId);

            Assert.AreEqual(category.Name, createdCategory?.Name);
            Assert.AreEqual(category.Description, createdCategory?.Description);
        }

        [TestMethod]
        public async Task ShouldUpdateCategory()
        {
            var category = DataHelper.GetCategory();
            string orignalName = category.Name;
            string orignalDescription = category.Description;
            var createdCategory = await DataHelper.CategoryService.Create(category);

            createdCategory.Name = "Updated Category";
            createdCategory.Description = "Updated Description";
            var updatedCategory = await DataHelper.CategoryService.Update(createdCategory, createdCategory.CategoryId);

            Assert.IsNotNull(updatedCategory);
            updatedCategory = DataHelper2.CategoryService.GetById(updatedCategory.CategoryId);
            Assert.IsNotNull(updatedCategory);
            Assert.AreNotEqual(orignalName, updatedCategory.Name);
            Assert.AreNotEqual(orignalDescription, updatedCategory.Description);
            Assert.AreEqual(createdCategory.Name, updatedCategory.Name);
            Assert.AreEqual(createdCategory.Description, updatedCategory.Description);
        }

        [TestMethod]
        public async Task ShouldDeleteCategory()
        {
            var category = DataHelper.GetCategory();

            var createdCategory = await DataHelper.CategoryService.Create(category);

            DataHelper.CategoryService.Delete(createdCategory.CategoryId);
            var deletedCategory = DataHelper2.CategoryService.GetById(createdCategory.CategoryId);

            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        public async Task ShouldReturnAllCategories()
        {
            await DataHelper.CategoryService.Create(DataHelper.GetCategory());
            await DataHelper.CategoryService.Create(DataHelper.GetCategory());
            var result = DataHelper2.CategoryService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }
    }
}