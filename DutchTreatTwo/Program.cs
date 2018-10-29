using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DutchTreatTwo.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateWebHostBuilder(args).Build();

           RunSeeding(host);
           host.Run();
      }

       private static void RunSeeding(IWebHost host)
       {
         var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

          using (var scope = scopeFactory.CreateScope())
          {
             var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
             seeder.SeedAsync().Wait();
          }
      }

       public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
