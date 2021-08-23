using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Services.Catalog.Entities;
using UdemyMicroservices.Services.Catalog.Settings;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var list = await _categoryCollection.Find(category => true).ToListAsync();

            return Response<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(list), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var result = await _categoryCollection.Find(category => category.Id == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return Response<CategoryDto>.Fail("Category not found", 404);
            }

            return Response<CategoryDto>.Success(_mapper.Map<CategoryDto>(result), 200);
        }

        public async Task<Response<CategoryDto>> CreateOrUpdateAsync(CategoryDto categoryDto)
        {
            // create
            if (string.IsNullOrEmpty(categoryDto.Id))
            {
                var newCategory = _mapper.Map<Category>(categoryDto);

                await _categoryCollection.InsertOneAsync(newCategory);

                categoryDto = _mapper.Map<CategoryDto>(newCategory);
            }
            // update
            else
            {
                var updateCategory = _mapper.Map<Category>(categoryDto);
                var result = await _categoryCollection.FindOneAndReplaceAsync(category => category.Id == categoryDto.Id, updateCategory);

                if (result == null)
                {
                    return Response<CategoryDto>.Fail("Category not found", 404);
                }
            }

            return Response<CategoryDto>.Success(categoryDto, 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _categoryCollection.DeleteOneAsync(category => category.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Category not found", 404);
            }
        }
    }
}