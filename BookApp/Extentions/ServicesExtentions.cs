using Microsoft.EntityFrameworkCore;
using Repositories.EfCore;

namespace BookApp.Extentions
{
    public static class ServicesExtentions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

        }
    }
}
