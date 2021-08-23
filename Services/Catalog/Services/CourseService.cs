using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Services.Catalog.Entities;
using UdemyMicroservices.Services.Catalog.Settings;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var list = await _courseCollection.Find(course => true).ToListAsync();

            if (list.Any())
            {
                foreach (var course in list)
                {
                    course.Category = await GetCategory(course.CategoryId);
                }
            }
            else
            {
                list = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(list), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var list = await _courseCollection.Find(course => course.UserId == userId).ToListAsync();

            if (list.Any())
            {
                foreach (var course in list)
                {
                    course.Category = await GetCategory(course.CategoryId);
                }
            }
            else
            {
                list = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(list), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var result = await _courseCollection.Find(course => course.Id == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return Response<CourseDto>.Fail("Course not found", 404);
            }

            result.Category = await GetCategory(result.CategoryId);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(result), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);

            newCourse.CreatedTime = DateTime.UtcNow;

            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(course => course.Id == courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(course => course.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }
        }

        private async Task<Category> GetCategory(string categoryId)
        {
            return await _categoryCollection.Find(category => category.Id == categoryId).FirstAsync();
        }
    }
}