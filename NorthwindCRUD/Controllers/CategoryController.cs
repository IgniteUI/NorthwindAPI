namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger logger;

        public CategoryController(CategoryService categoryService, IMapper mapper, ILogger logger)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.logger = logger;   
        }

        [HttpGet]
        [Authorize]
        public ActionResult<CategoryInputModel[]> GetAll()
        {
            try
            {
                var categories = this.categoryService.GetAll();
                return Ok(this.mapper.Map<CategoryDb[], CategoryInputModel[]>(categories));
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
            
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CategoryInputModel> GetById(int id)
        {
            try
            {
                var category = this.categoryService.GetById(id);

                if (category != null)
                {
                    return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult<CategoryInputModel> Create(CategoryInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
                    var category = this.categoryService.Create(mappedModel);
                    return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<CategoryInputModel> Update(CategoryInputModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mappedModel = this.mapper.Map<CategoryInputModel, CategoryDb>(model);
                    var category = this.categoryService.Update(mappedModel);
                    return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<CategoryInputModel> Delete(int id)
        {
            try
            {
                var category = this.categoryService.Delete(id);

                if (category != null)
                {
                    return Ok(this.mapper.Map<CategoryDb, CategoryInputModel>(category));
                }

                return NotFound();
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}