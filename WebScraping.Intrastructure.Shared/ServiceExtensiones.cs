using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebScraping.Core.Application.Constants;
using WebScraping.Infrastructure.Email.Service;
using WebScraping.Infrastructure.Email.Settings;

namespace WebScraping.Infrastructure.Email
{
    public static class ServiceExtensiones
    {
        public static void AddInfrastructureEmail(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            service.AddTransient<IEmailServiceAsync, EmailServiceAsync>();
        }
    }
}