using Salon.DataContext;
using Microsoft.AspNetCore.Mvc.Formatters;
using Salon.WebApi.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Salon.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("https://localhost:5002/");

            // Add services to the container.
            builder.Services.AddSalonContext();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //используем WorkerRepository в качестве зависимости с ограниченной областью действия
            builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();

            //используем WorkerRepository в качестве зависимости с ограниченной областью действия
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(options =>
            {
                options.WithMethods("GET", "POST", "PUT", "DELETE");
                options.WithOrigins("https://localhost:5001/");
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c=>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json",
                        "Salon Web Api Version 1");
                    c.SupportedSubmitMethods(new[]
                    {
                        SubmitMethod.Get, SubmitMethod.Post,
                        SubmitMethod.Put, SubmitMethod.Delete
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}