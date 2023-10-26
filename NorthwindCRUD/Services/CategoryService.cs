namespace NorthwindCRUD.Services
{
    using NorthwindCRUD.Helpers;
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

        public CategoryDb? GetById(int id)
        {
            return this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public CategoryDb Create(CategoryDb model)
        {
            var id = IdGenerator.CreateDigitsId();
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateDigitsId();
                existWithId = this.GetById(id);
            }

            model.CategoryId = id;

            PropertyHelper<CategoryDb>.MakePropertiesEmptyIfNull(model);

            var categoryEntity = this.dataContext.Categories.Add(model);
            this.dataContext.SaveChanges();

            return categoryEntity.Entity;
        }

        public CategoryDb? Update(CategoryDb model)
        {
            var categoryEntity = this.dataContext.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId);
            if (categoryEntity != null)
            {
                categoryEntity.Description = model.Description != null ? model.Description : categoryEntity.Description;
                categoryEntity.Name = model.Name != null ? model.Name : categoryEntity.Name;
                categoryEntity.Picture = model.Picture != null ? model.Picture : categoryEntity.Picture;

                this.dataContext.SaveChanges();
            }

            return categoryEntity;
        }

        public CategoryDb? Delete(int id)
        {
            var categoryEntity = this.GetById(id);
            if (categoryEntity != null)
            {
                this.dataContext.Categories.Remove(categoryEntity);
                this.dataContext.SaveChanges();
            }

            return categoryEntity;
        }
    }
}
