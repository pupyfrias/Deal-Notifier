using DealNotifier.Infrastructure.Email.Service;
using DealNotifier.Infrastructure.Email.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DealNotifier.Core.Application.Constants;

namespace DealNotifier.Infrastructure.Email
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