using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreApi.Models;

namespace MovieStoreApi.Models
{
    public class Order
    {
        public int Id {get;set;}
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int MovieId{get;set;}
        public virtual Movie Movie {get;set;}

        public decimal Price { get; set; }
        public DateTime PurchaseDate  { get; set; }
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.MovieId).IsRequired();
        builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.PurchaseDate).IsRequired();
    }
}