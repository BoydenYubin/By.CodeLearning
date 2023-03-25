using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ByLerning.SignalR
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
                   webBuilder.UseKestrel(options =>
                   {
                       options.ConfigureHttpsDefaults(options => 
                       {
                           options.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(@"D:\unilumin.ucb41.pfx", "dolbyuni");
                           //options.SslProtocols = System.Security.Authentication.SslProtocols.Tls;
                           options.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
                           options.ClientCertificateValidation = (certificate, chain, errors) =>
                           {
                               return true;
                           };
                       });
                   });
               })
               .ConfigureLogging(builder => 
               {
                   
               });
        }
    }
}
