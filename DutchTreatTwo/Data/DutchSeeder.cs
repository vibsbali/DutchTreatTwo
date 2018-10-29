using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatTwo.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DutchTreatTwo.Data
{
    public class DutchSeeder
    {
       private readonly DutchContext _context;
       private readonly IHostingEnvironment _hostingEnvironment;
       private readonly UserManager<StoreUser> _userManager;

       public DutchSeeder(DutchContext context, IHostingEnvironment hostingEnvironment, UserManager<StoreUser> userManager)
       {
          _context = context;
          _hostingEnvironment = hostingEnvironment;
          _userManager = userManager;
       }

       public async Task SeedAsync()
       {
          _context.Database.EnsureCreated();

          var user = await _userManager.FindByEmailAsync("test@test.com");
          if (user == null)
          {
             user = new StoreUser
             {
                FirstName = "test",
                LastName = "test",
                Email = "test@test.com",
                UserName = "test@test.com"
             };

             var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
             if (result != IdentityResult.Success)
             {
                throw new Exception("Could not create the user");
             }
          }

          if (!_context.Products.Any())
          {
             var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data/art.json");
             var json = File.ReadAllText(filePath);
             var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
             _context.Products.AddRange(products);

             var order = _context.Orders.FirstOrDefault(o => o.Id == 1);
             if (order != null)
             {
                order.User = user;
                order.Items = new List<OrderItem>()
                {
                   new OrderItem
                   {
                      Product = products.First(),
                      Quantity = 5,
                      UnitPrice = products.First().Price
                   }
                };
             }
             _context.SaveChanges();
          }
       }
    }
}
