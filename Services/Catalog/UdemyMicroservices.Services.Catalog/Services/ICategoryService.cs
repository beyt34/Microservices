using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> GetByIdAsync(string id);
        Task<Response<CategoryDto>> CreateOrUpdateAsync(CategoryDto categoryDto);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}