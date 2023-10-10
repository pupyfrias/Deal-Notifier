﻿using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DealNotifier.Infrastructure.Persistence.Setup
{
    public static class DbContextSetup
    {
        public static readonly Func<IConfiguration, Action<DbContextOptionsBuilder>> Configure = configuration =>
        {
            Action<DbContextOptionsBuilder> Options = (options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DealNotifierConnection"),
                            optionAction => optionAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            };

            return Options;
        };

        public static readonly Action<DbContextOptionsBuilder> InMemoryOptions = (options) =>
        {
            options.UseInMemoryDatabase("ApplicationDb");
        };
    }
}