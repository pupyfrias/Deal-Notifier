using System.Net;
using System.Net.Sockets;

namespace WebScraping.Infrastructure.Identity.Helpers
{
    public class IpHelper
    {
        public static string GetIpAddress()
        {
            var host = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in host)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
    }
}