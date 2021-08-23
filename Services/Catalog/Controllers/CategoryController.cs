using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Services.Catalog.Services;
using UdemyMicroservices.Shared.ControllerBases;

namespace UdemyMicroservices.Services.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return CreateActionResultInstance(response);
        }

        [HttpPost]
        [Route("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate(CategoryDto categoryDto)
        {
            var response = await _categoryService.CreateOrUpdateAsync(categoryDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _categoryService.DeleteAsync(id);
            return CreateActionResultInstance(response);
        }
    }
}
