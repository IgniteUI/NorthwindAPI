using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindCRUD.Tests
{
    [TestClass]
    public class CategoryServiceFixture : BaseFixture
    {
        [TestMethod]
        public void ShouldCreateCategory()
        {
            var category = DataHelper.GetCategory();
            var createdCategory = DataHelper.CategoryService.Create(category);

            var result = DataHelper.CategoryService.GetById(createdCategory.CategoryId);

            Assert.IsNotNull(createdCategory);
            Assert.AreEqual(category.Name, result.Name);
            Assert.AreEqual(category.Description, result.Description);
        }

        [TestMethod]
        public void ShouldUpdateCategory()
        {
            var category = DataHelper.GetCategory();
            var createdCategory = DataHelper.CategoryService.Create(category);

            createdCategory.Name = "Updated Category";
            createdCategory.Description = "Updated Description";
            var updatedCategory = DataHelper.CategoryService.Update(createdCategory);

            Assert.IsNotNull(updatedCategory);
            Assert.AreEqual("Updated Category", updatedCategory.Name);
            Assert.AreEqual("Updated Description", updatedCategory.Description);
        }

        [TestMethod]
        public void ShouldDeleteCategory()
        {
            var category = DataHelper.GetCategory();

            var createdCategory = DataHelper.CategoryService.Create(category);

            DataHelper.CategoryService.Delete(createdCategory.CategoryId);
            var deletedCategory = DataHelper.CategoryService.GetById(createdCategory.CategoryId);

            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        public void ShouldReturnAllCategories()
        {
            DataHelper.CategoryService.Create(DataHelper.GetCategory());
            DataHelper.CategoryService.Create(DataHelper.GetCategory());
            var result = DataHelper.CategoryService.GetAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
        }
    }
}