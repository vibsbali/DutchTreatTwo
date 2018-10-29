using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatTwo.Data.Entities;

namespace DutchTreatTwo.ViewModels
{
   public class OrderViewModel
   {
      public int OrderId { get; set; }
      public DateTime OrderDate { get; set; }

      [Required]
      [MinLength(4)]
      public string OrderNumber { get; set; }

      public ICollection<OrderItemViewModel> Items { get; set; }
   }

   public class OrderItemViewModel
   {
      public int Id { get; set; }

      [Required]
      public int Quantity { get; set; }

      [Required]
      public decimal UnitPrice { get; set; }

      [Required]
      public int ProductId { get; set; }
      //By adding Product i.e. entity the property is referring to
      //we set automapper to read the tree without having us to 
      //setup in Mappings explicitly
      public string ProductCategory { get; set; }
      public string ProductSize { get; set; }
      public string ProductTitle { get; set; }
      public string ProductArtist { get; set; }
      public string ProductArtId { get; set; }
   }
}
