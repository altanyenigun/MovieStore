using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStoreApi.Models;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public bool isActive { get; set; }

    public virtual List<Genre> Genres { get; set; }
    public virtual Director Director { get; set; }
    public virtual List<Actor> Actors { get; set; }
    public virtual List<Customer> Customers {get;set;}
}

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2).HasDefaultValue(0);
        builder.Property(x => x.isActive).IsRequired();

        builder.HasMany(e => e.Actors)
            .WithMany(e => e.Movies);

        builder.HasMany(e => e.Genres)
            .WithMany(e => e.Movies);
    }
}


