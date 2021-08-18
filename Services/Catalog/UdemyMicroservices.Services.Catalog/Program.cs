using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UdemyMicroservices.Services.Catalog.Dtos;
using UdemyMicroservices.Services.Catalog.Services;

namespace UdemyMicroservices.Services.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

                if (!categoryService.GetAllAsync().Result.Data.Any())
                {
                    categoryService.CreateOrUpdateAsync(new CategoryDto { Name = "API" }).Wait();
                    categoryService.CreateOrUpdateAsync(new CategoryDto { Name = "Web" }).Wait();
                    categoryService.CreateOrUpdateAsync(new CategoryDto { Name = "Db" }).Wait();
                    categoryService.CreateOrUpdateAsync(new CategoryDto { Name = "Cache" }).Wait();
                    categoryService.CreateOrUpdateAsync(new CategoryDto { Name = "Docker" }).Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
