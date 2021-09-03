using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ProductiveBoard.Areas.Identity.IdentityHostingStartup))]
namespace ProductiveBoard.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}