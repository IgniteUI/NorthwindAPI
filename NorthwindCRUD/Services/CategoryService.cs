namespace NorthwindCRUD.Services
{
    using NorthwindCRUD.Models.DbModels;

    public class CategoryService
    {
        private readonly DataContext dataContext;

        public CategoryService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public CategoryDb[] GetAll()
        {
            return this.dataContext.Categories.ToArray();
        }

        public CategoryDb GetById(int id)
        {
            return this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public CategoryDb Create(CategoryDb model)
        {
            var categoryEntity = this.dataContext.Categories.Add(model);
            this.dataContext.SaveChanges();

            return categoryEntity.Entity;
        }

        public CategoryDb Update(CategoryDb model)
        {
            var categoryEntity = this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId);
            if (categoryEntity != null)
            {
                categoryEntity.Description = model.Description;
                categoryEntity.Name = model.Name;

                this.dataContext.SaveChanges();
            }

            return categoryEntity;
        }

        public CategoryDb Delete(int id)
        {
            var categoryEntity = this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (categoryEntity != null)
            {
                this.dataContext.Categories.Remove(categoryEntity);
                this.dataContext.SaveChanges();
            }

            return categoryEntity;
        }
    }
}
