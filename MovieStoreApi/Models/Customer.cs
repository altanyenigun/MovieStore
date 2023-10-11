using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStoreApi.Models;

namespace MovieStoreApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public virtual List<Movie> Movies { get; set; }
        public virtual List<Genre> Genres { get; set; }
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Role).IsRequired().HasMaxLength(50);

        builder.HasMany(e => e.Movies)
            .WithMany(e => e.Customers);

        builder.HasMany(e => e.Genres)
            .WithMany(e => e.Customers);
    }
}