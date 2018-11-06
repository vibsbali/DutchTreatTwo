using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatTwo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo.Data
{
   public class DutchRepository : IDutchRepository
   {
      private readonly DutchContext _ctx;
      private readonly ILogger<DutchRepository> _logger;

      public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
      {
         _ctx = ctx;
         _logger = logger;
      }

      public async Task<IEnumerable<Product>> GetAllProducts()
      {
         _logger.LogInformation("Getting all products");
         try
         {
            var allProducts = new List<Product>();

            var query = _ctx.Products.GroupBy(p => p.Title)
               .ForEachAsync(products =>
               {
                  var productGroup = products.Take(1);
                  allProducts.Add(productGroup.First());
               });

            await query;

            return allProducts;
         }
         catch (Exception ex)
         {
            _logger.LogError($"Failed to get all products {ex}");
            return null;
         }

         //return _ctx.Products.OrderBy(p => p.Title).ToList();
      }

      public IEnumerable<Product> GetProductsByCategory(string category)
      {
         return _ctx.Products.Where(p => p.Category == category).ToList();
      }

      public bool SaveAll()
      {
         return _ctx.SaveChanges() > 0;
      }

      public IEnumerable<Order> GetAllOrders(bool includeItems = true)
      {
         IQueryable<Order> query = _ctx.Orders;

         if (includeItems)
         {
            query = query.Include(o => o.Items).ThenInclude(i => i.Product)
               .Include(o => o.User);
         }
         else
         {
            query.Include(o => o.User);
         }

         return query.ToList();
      }

      public void AddEntity(object model)
      {
         _ctx.Add(model);
      }

      public IEnumerable<Order> GetAllOrderByUser(string user, bool includeItems)
      {
         IQueryable<Order> query = _ctx.Orders.Where(o => o.User.UserName == user);

         if (includeItems)
         {
            query = query.Include(o => o.Items).ThenInclude(i => i.Product)
               .Include(o => o.User);
         }
         else
         {
            query.Include(o => o.User);
         }

         return query.ToList();
      }

      public Order GetOrderById(string userName, int orderId)
      {
         return _ctx.Orders.Where(o => o.User.UserName == userName)
            .Include(o => o.Items)
            .Include(o => o.User)
            .SingleOrDefault(o => o.Id == orderId);
      }

       public void AddOrder(Order newOrder)
       {
           //convert new products to lookup of product
           foreach (var newOrderItem in newOrder.Items)
           {
               newOrderItem.Product = _ctx.Products.Find(newOrderItem.Product.Id);
           }

           AddEntity(newOrder);
       }
   }

   public interface IDutchRepository
   {
      Task<IEnumerable<Product>> GetAllProducts();
      IEnumerable<Product> GetProductsByCategory(string category);
      bool SaveAll();
      IEnumerable<Order> GetAllOrders(bool includeItems = true);
      void AddEntity(object order);
      IEnumerable<Order> GetAllOrderByUser(string user, bool includeItems);
      Order GetOrderById(string userName, int orderId);
       void AddOrder(Order newOrder);
   }
}
