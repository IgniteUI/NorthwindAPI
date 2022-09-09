namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.InputModels;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categoryService;
        private readonly IMapper mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;   
        }

        [HttpGet]
        public ActionResult<CategoryInputModel[]> GetAll()
        {
            var categories = this.categoryService.GetAll();
            return Ok(this.mapper.Map<CategoryDb[], CategoryInputModel[]>(categories));
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryInputModel> GetById(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category != null)
            {
                return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<CategoryInputModel> Create(CategoryInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
                var category = this.categoryService.Create(mappedModel);
                return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public ActionResult<CategoryInputModel> Update(CategoryInputModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
                var category = this.categoryService.Update(mappedModel);
                return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoryInputModel> Delete(int id)
        {
            var category = this.categoryService.Delete(id);

            if (category != null)
            {
                return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
            }

            return NotFound();
        }
    }
}