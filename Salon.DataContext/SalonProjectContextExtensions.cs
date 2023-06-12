using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; //UseSqlServer
using Microsoft.Extensions.DependencyInjection; //IServiceCollection

namespace Salon.DataContext
{
    public static class SalonProjectContextExtensions
    {
        /// <summary>
        /// Добавляет контекст базы данных SalonProject в IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Устанавливаем строку подключения.</param>
        /// <returns>IServiceCollection для добавления других сервисов.</returns>
        public static IServiceCollection AddSalonContext(
          this IServiceCollection services, string connectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=SalonProject;"
            + "Integrated Security=true;MultipleActiveResultsets=true;")
        {
            services.AddDbContext<SalonProjectContext>(options =>
            options.UseSqlServer(connectionString));
            return services;
        }
    }
}
