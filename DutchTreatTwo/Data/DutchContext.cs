﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreatTwo.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutchTreatTwo.Data
{
   public class DutchContext : IdentityDbContext<StoreUser>
   {
      public DutchContext(DbContextOptions<DutchContext> options)
      : base(options)
      {
      }

      public DbSet<Product> Products { get; set; }
      public DbSet<Order> Orders { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<Order>()
            .HasData(new Order()
            {
               Id = 1,
               OrderDate = DateTime.UtcNow,
               OrderNumber = "12345"
            });
      }
   }
}
