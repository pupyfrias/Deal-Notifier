using Catalog.Application.Interfaces.Services;
using Email.Service;
using Email.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Email
{
    public static class ServiceExtensiones
    {
        public static void AddInfrastructureEmail(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            service.AddScoped<IEmailService, EmailService>();
        }
    }
}