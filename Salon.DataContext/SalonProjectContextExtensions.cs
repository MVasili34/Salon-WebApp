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
        /// Adds NorthwindContext to the specified IServiceCollection. Uses the SqlServer database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Set to override the default.</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
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
