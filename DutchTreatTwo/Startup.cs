using System.Linq;
using System.Text;
using AutoMapper;
using DutchTreatTwo.Data;
using DutchTreatTwo.Data.Entities;
using DutchTreatTwo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DutchTreatTwo
{
   public class Startup
   {
      private readonly IConfiguration _configuration;

      public Startup(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddIdentity<StoreUser, IdentityRole>(cfg =>
         {
            cfg.User.RequireUniqueEmail = true;
         }).AddEntityFrameworkStores<DutchContext>();

         services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(cfg =>
            {
               cfg.TokenValidationParameters = new TokenValidationParameters()
               {
                  ValidIssuer = _configuration["Tokens:Issuer"],
                  ValidAudience = _configuration["Tokens:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
               };
            });

         services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

         services.AddAutoMapper();
         services.AddTransient<IMailService, NullMailService>();
         services.AddTransient<DutchSeeder>();
         services.AddScoped<IDutchRepository, DutchRepository>();



         services.AddDbContext<DutchContext>(cfg =>
            {
               cfg.UseSqlServer(_configuration.GetConnectionString("DutchConnectionString"));
            });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/error");
         }

         app.UseStaticFiles();

         app.UseNodeModules(env);

         app.UseAuthentication();

         app.UseMvc(cfg =>
         {
            cfg.MapRoute("Default", "/{controller}/{action}/{id?}", new { Controller = "App", Action = "Index" });
         });
      }
   }
}
