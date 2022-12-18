using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence
{
    public static class DbContext
    {
        public static Func<IConfiguration, Action<DbContextOptionsBuilder>> Options = configuration =>
        {
            Action<DbContextOptionsBuilder> Options = (options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            optionAction => optionAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            };

            return Options;
        };

        public static Action<DbContextOptionsBuilder> InMemoryOptions = (options) =>
        {
            options.UseInMemoryDatabase("ApplicationDb");
        };   
    }
}
