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

            // Add services to the container.
            builder.Services.AddSalonContext();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //���������� WorkerRepository � �������� ����������� � ������������ �������� ��������
            builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();

            //���������� WorkerRepository � �������� ����������� � ������������ �������� ��������
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

            var app = builder.Build();

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