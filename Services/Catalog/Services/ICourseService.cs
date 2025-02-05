﻿using System.Collections.Generic;
using System.Threading.Tasks;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDto>>> GetAllAsync();

        Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);

        Task<Response<CourseDto>> GetByIdAsync(string id);

        Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);

        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto);

        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
