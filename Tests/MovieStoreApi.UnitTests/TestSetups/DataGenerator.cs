using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieStoreApi.Models;

namespace MovieStoreApi.UnitTests.TestSetups
{
    public static class DataGenerator
    {
        public static async void AddData(this FakeDataContext context)
        {
            // Actors
            await context.Actors.AddRangeAsync(
                new Actor { Name = "John", Surname = "Doe" },
                new Actor { Name = "Jane", Surname = "Doe" },
                new Actor { Name = "Tom", Surname = "Hanks" },
                new Actor { Name = "Brad", Surname = "Pitt" },
                new Actor { Name = "Angelina", Surname = "Jolie" },
                new Actor { Name = "Leonardo", Surname = "DiCaprio" },
                new Actor { Name = "Meryl", Surname = "Streep" },
                new Actor { Name = "Denzel", Surname = "Washington" },
                new Actor { Name = "Julia", Surname = "Roberts" },
                new Actor { Name = "Johnny", Surname = "Depp" }
            );

            // Directors
            await context.Directors.AddRangeAsync(
                new Director { Name = "Steven", Surname = "Spielberg" },
                new Director { Name = "Martin", Surname = "Scorsese" },
                new Director { Name = "Quentin", Surname = "Tarantino" },
                new Director { Name = "Christopher", Surname = "Nolan" },
                new Director { Name = "David", Surname = "Fincher" },
                new Director { Name = "Francis Ford", Surname = "Coppola" },
                new Director { Name = "Clint", Surname = "Eastwood" },
                new Director { Name = "Alfred", Surname = "Hitchcock" },
                new Director { Name = "Stanley", Surname = "Kubrick" },
                new Director { Name = "Tim", Surname = "Burton" }
            );

            // Genres
            await context.Genres.AddRangeAsync(
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Mystery" },
                new Genre { Name = "Fantasy" }
            );

            // Customers
            await context.Customers.AddRangeAsync(
                new Customer { Username = "admin", Name = "admin", Surname = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), Role = "Admin" },
                new Customer { Username = "altan", Name = "altan", Surname = "altan", PasswordHash = BCrypt.Net.BCrypt.HashPassword("altan"), Role = "Customer" },
                new Customer { Username = "tom_hanks", Name = "Tom", Surname = "Hanks", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "brad_pitt", Name = "Brad", Surname = "Pitt", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "angelina_jolie", Name = "Angelina", Surname = "Jolie", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "leo_dicaprio", Name = "Leonardo", Surname = "DiCaprio", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "meryl_streep", Name = "Meryl", Surname = "Streep", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "denzel_washington", Name = "Denzel", Surname = "Washington", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "julia_roberts", Name = "Julia", Surname = "Roberts", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" },
                new Customer { Username = "johnny_depp", Name = "Johnny", Surname = "Depp", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = "Customer" }
            );

            // Movies

            await context.Movies.AddRangeAsync(
                new Movie { Name = "Movie 1", Year = 2021, Price = 9.99m, isActive = true, Director = context.Directors.Find(1)},
                new Movie { Name = "Movie 2", Year = 2022, Price = 10.99m, isActive = true, Director = context.Directors.Find(2) },
                new Movie { Name = "Movie 3", Year = 2023, Price = 11.99m, isActive = true, Director = context.Directors.Find(3) },
                new Movie { Name = "Movie 4", Year = 2024, Price = 12.99m, isActive = true, Director = context.Directors.Find(4) },
                new Movie { Name = "Movie 5", Year = 2025, Price = 13.99m, isActive = true, Director = context.Directors.Find(5)},
                new Movie { Name = "Movie 6", Year = 2026, Price = 14.99m, isActive = true, Director = context.Directors.Find(6) },
                new Movie { Name = "Movie 7", Year = 2027, Price = 15.99m, isActive = true, Director = context.Directors.Find(7) },
                new Movie { Name = "Movie 8", Year = 2028, Price = 16.99m, isActive = true, Director = context.Directors.Find(8) },
                new Movie { Name = "Movie 9", Year = 2029, Price = 17.99m, isActive = true, Director = context.Directors.Find(9) },
                new Movie { Name = "Movie 10", Year = 2030, Price = 18.99m, isActive = true, Director = context.Directors.Find(1) }
            );

            // Orders
            await context.Orders.AddRangeAsync(
                new Order { CustomerId = 1, MovieId = 1, Price = 9.99m, PurchaseDate = new DateTime(2023, 10, 17) },
                new Order { CustomerId = 1, MovieId = 2, Price = 12.99m, PurchaseDate = new DateTime(2023, 10, 18) },
                new Order { CustomerId = 1, MovieId = 3, Price = 14.99m, PurchaseDate = new DateTime(2023, 10, 19) },
                new Order { CustomerId = 2, MovieId = 3, Price = 14.99m, PurchaseDate = new DateTime(2023, 10, 20) },
                new Order { CustomerId = 2, MovieId = 4, Price = 11.99m, PurchaseDate = new DateTime(2023, 10, 21) },
                new Order { CustomerId = 2, MovieId = 5, Price = 10.99m, PurchaseDate = new DateTime(2023, 10, 22) },
                new Order { CustomerId = 3, MovieId = 5, Price = 10.99m, PurchaseDate = new DateTime(2023, 10, 23) },
                new Order { CustomerId = 3, MovieId = 6, Price = 16.99m, PurchaseDate = new DateTime(2023, 10, 24) },
                new Order { CustomerId = 3, MovieId = 7, Price = 13.99m, PurchaseDate = new DateTime(2023, 10, 25) },
                new Order { CustomerId = 4, MovieId = 7, Price = 13.99m, PurchaseDate = new DateTime(2023, 10, 26) },
                new Order { CustomerId = 4, MovieId = 8, Price = 17.99m, PurchaseDate = new DateTime(2023, 10, 27) },
                new Order { CustomerId = 4, MovieId = 9, Price = 19.99m, PurchaseDate = new DateTime(2023, 10, 28) },
                new Order { CustomerId = 5, MovieId = 9, Price = 19.99m, PurchaseDate = new DateTime(2023, 10, 29) },
                new Order { CustomerId = 5, MovieId = 10, Price = 15.99m, PurchaseDate = new DateTime(2023, 10, 30) },
                new Order { CustomerId = 5, MovieId = 1, Price = 14.99m, PurchaseDate = new DateTime(2023, 10, 31) },
                new Order { CustomerId = 6, MovieId = 1, Price = 15.99m, PurchaseDate = new DateTime(2023, 11, 1) },
                new Order { CustomerId = 6, MovieId = 2, Price = 12.99m, PurchaseDate = new DateTime(2023, 11, 2) },
                new Order { CustomerId = 6, MovieId = 3, Price = 13.99m, PurchaseDate = new DateTime(2023, 11, 3) },
                new Order { CustomerId = 7, MovieId = 3, Price = 10.99m, PurchaseDate = new DateTime(2023, 11, 4) },
                new Order { CustomerId = 7, MovieId = 4, Price = 9.99m, PurchaseDate = new DateTime(2023, 11, 5) },
                new Order { CustomerId = 7, MovieId = 5, Price = 18.99m, PurchaseDate = new DateTime(2023, 11, 6) },
                new Order { CustomerId = 8, MovieId = 5, Price = 16.99m, PurchaseDate = new DateTime(2023, 11, 7) },
                new Order { CustomerId = 8, MovieId = 6, Price = 12.99m, PurchaseDate = new DateTime(2023, 11, 8) },
                new Order { CustomerId = 8, MovieId = 7, Price = 11.99m, PurchaseDate = new DateTime(2023, 11, 9) },
                new Order { CustomerId = 9, MovieId = 7, Price = 9.99m, PurchaseDate = new DateTime(2023, 11, 10) },
                new Order { CustomerId = 9, MovieId = 8, Price = 17.99m, PurchaseDate = new DateTime(2023, 11, 11) },
                new Order { CustomerId = 9, MovieId = 9, Price = 18.99m, PurchaseDate = new DateTime(2023, 11, 12) },
                new Order { CustomerId = 10, MovieId = 9, Price = 14.99m, PurchaseDate = new DateTime(2023, 11, 13) },
                new Order { CustomerId = 10, MovieId = 10, Price = 15.99m, PurchaseDate = new DateTime(2023, 11, 14) },
                new Order { CustomerId = 10, MovieId = 1, Price = 16.99m, PurchaseDate = new DateTime(2023, 11, 15) }
            );

            //
            var customer1 = context.Customers.Find(1)!;
            customer1.Movies.Add(new Movie { Id = 1 }); // 1. müşteri 1. filmi seviyor
            customer1.Movies.Add(new Movie { Id = 2 }); // 1. müşteri 2. filmi seviyor
            customer1.Movies.Add(new Movie { Id = 3 }); // 1. müşteri 3. filmi seviyor
            customer1.Genres.Add(new Genre { Id = 1 }); // 1. müşteri 1. türü seviyor
            customer1.Genres.Add(new Genre { Id = 2 }); // 1. müşteri 2. türü seviyor
            customer1.Genres.Add(new Genre { Id = 3 }); // 1. müşteri 3. türü seviyor

            var customer2 = context.Customers.Find(2)!;
            customer2.Movies.Add(new Movie { Id = 3 }); // 2. müşteri 3. filmi seviyor
            customer2.Movies.Add(new Movie { Id = 4 }); // 2. müşteri 4. filmi seviyor
            customer2.Movies.Add(new Movie { Id = 5 }); // 2. müşteri 5. filmi seviyor
            customer2.Genres.Add(new Genre { Id = 3 }); // 2. müşteri 3. türü seviyor
            customer2.Genres.Add(new Genre { Id = 4 }); // 2. müşteri 4. türü seviyor
            customer2.Genres.Add(new Genre { Id = 5 }); // 2. müşteri 5. türü seviyor

            var customer3 = context.Customers.Find(3)!;
            customer3.Movies.Add(new Movie { Id = 5 }); // 3. müşteri 5. filmi seviyor
            customer3.Movies.Add(new Movie { Id = 6 }); // 3. müşteri 6. filmi seviyor
            customer3.Movies.Add(new Movie { Id = 7 }); // 3. müşteri 7. filmi seviyor
            customer3.Genres.Add(new Genre { Id = 2 }); // 2. müşteri 2. türü seviyor


            var customer4 = context.Customers.Find(4)!;
            customer4.Movies.Add(new Movie { Id = 7 }); // 4. müşteri 7. filmi seviyor
            customer4.Movies.Add(new Movie { Id = 8 }); // 4. müşteri 8. filmi seviyor
            customer4.Movies.Add(new Movie { Id = 9 }); // 4. müşteri 9. filmi seviyor
            customer4.Genres.Add(new Genre { Id = 6 }); // 4. müşteri 6. türü seviyor
            customer4.Genres.Add(new Genre { Id = 7 }); // 4. müşteri 7. türü seviyor

            var customer5 = context.Customers.Find(5)!;
            customer5.Movies.Add(new Movie { Id = 9 }); // 5. müşteri 9. filmi seviyor
            customer5.Movies.Add(new Movie { Id = 10 }); // 5. müşteri 10. filmi seviyor
            customer5.Movies.Add(new Movie { Id = 1 }); // 5. müşteri 1. filmi seviyor
            customer5.Genres.Add(new Genre { Id = 8 }); // 5. müşteri 8. türü seviyor
            customer5.Genres.Add(new Genre { Id = 9 }); // 5. müşteri 9. türü seviyor

            var customer6 = context.Customers.Find(6)!;
            customer6.Movies.Add(new Movie { Id = 1 }); // 6. müşteri 1. filmi seviyor
            customer6.Movies.Add(new Movie { Id = 2 }); // 6. müşteri 2. filmi seviyor
            customer6.Movies.Add(new Movie { Id = 3 }); // 6. müşteri 3. filmi seviyor
            customer6.Genres.Add(new Genre { Id = 1 }); // 6. müşteri 1. türü seviyor
            customer6.Genres.Add(new Genre { Id = 3 }); // 6. müşteri 3. türü seviyor

            var customer7 = context.Customers.Find(7)!;
            customer7.Movies.Add(new Movie { Id = 3 }); // 7. müşteri 3. filmi seviyor
            customer7.Movies.Add(new Movie { Id = 4 }); // 7. müşteri 4. filmi seviyor
            customer7.Movies.Add(new Movie { Id = 5 }); // 7. müşteri 5. filmi seviyor
            customer7.Genres.Add(new Genre { Id = 2 }); // 7. müşteri 2. türü seviyor
            customer7.Genres.Add(new Genre { Id = 4 }); // 7. müşteri 4. türü seviyor

            var customer8 = context.Customers.Find(8)!;
            customer8.Movies.Add(new Movie { Id = 5 }); // 8. müşteri 5. filmi seviyor
            customer8.Movies.Add(new Movie { Id = 6 }); // 8. müşteri 6. filmi seviyor
            customer8.Movies.Add(new Movie { Id = 7 }); // 8. müşteri 7. filmi seviyor
            customer8.Genres.Add(new Genre { Id = 6 }); // 8. müşteri 6. türü seviyor
            customer8.Genres.Add(new Genre { Id = 8 }); // 8. müşteri 8. türü seviyor


            var customer9 = context.Customers.Find(9)!;
            customer9.Movies.Add(new Movie { Id = 7 }); // 9. müşteri 7. filmi seviyor
            customer9.Movies.Add(new Movie { Id = 8 }); // 9. müşteri 8. filmi seviyor
            customer9.Movies.Add(new Movie { Id = 9 }); // 9. müşteri 9. filmi seviyor
            customer9.Genres.Add(new Genre { Id = 1 }); // 9. müşteri 1. türü seviyor
            customer9.Genres.Add(new Genre { Id = 9 }); // 9. müşteri 9. türü seviyor

            var customer10 = context.Customers.Find(10)!;
            customer10.Movies.Add(new Movie { Id = 9 }); // 10. müşteri 9. filmi seviyor
            customer10.Movies.Add(new Movie { Id = 10 }); // 10. müşteri 10. filmi seviyor
            customer10.Movies.Add(new Movie { Id = 1 }); // 10. müşteri 1. filmi seviyor
            customer10.Genres.Add(new Genre { Id = 3 }); // 10. müşteri 3. türü seviyor
            customer10.Genres.Add(new Genre { Id = 5 }); // 10. müşteri 5. türü seviyor

            var movie1 = context.Movies.Find(1)!;
            movie1.Actors.Add(context.Actors.Find(1)); // 1. film için 1. aktörü ekliyorum
            movie1.Actors.Add(context.Actors.Find(2)); // 1. film için 2. aktörü ekliyorum
            movie1.Actors.Add(context.Actors.Find(3)); // 1. film için 3. aktörü ekliyorum

            var movie2 = context.Movies.Find(2)!;
            movie2.Actors.Add(context.Actors.Find(2)); // 2. film için 2. aktörü ekliyorum
            movie2.Actors.Add(context.Actors.Find(3)); // 2. film için 3. aktörü ekliyorum
            movie2.Actors.Add(context.Actors.Find(4)); // 2. film için 4. aktörü ekliyorum

            var movie3 = context.Movies.Find(3)!;
            movie3.Actors.Add(context.Actors.Find(4)); // 3. film için 4. aktörü ekliyorum
            movie3.Actors.Add(context.Actors.Find(5)); // 3. film için 5. aktörü ekliyorum
            movie3.Actors.Add(context.Actors.Find(6)); // 3. film için 6. aktörü ekliyorum

            var movie4 = context.Movies.Find(4)!;
            movie4.Actors.Add(context.Actors.Find(6)); // 4. film için 6. aktörü ekliyorum
            movie4.Actors.Add(context.Actors.Find(7)); // 4. film için 7. aktörü ekliyorum
            movie4.Actors.Add(context.Actors.Find(8)); // 4. film için 8. aktörü ekliyorum

            var movie5 = context.Movies.Find(5)!;
            movie5.Actors.Add(context.Actors.Find(8)); // 5. film için 8. aktörü ekliyorum
            movie5.Actors.Add(context.Actors.Find(9)); // 5. film için 9. aktörü ekliyorum
            movie5.Actors.Add(context.Actors.Find(10)); // 5. film için 10. aktörü ekliyorum

            var movie6 = context.Movies.Find(6)!;
            movie6.Actors.Add(context.Actors.Find(10)); // 6. film için 10. aktörü ekliyorum
            movie6.Actors.Add(context.Actors.Find(1)); // 6. film için 1. aktörü ekliyorum
            movie6.Actors.Add(context.Actors.Find(2)); // 6. film için 2. aktörü ekliyorum

            var movie7 = context.Movies.Find(7)!;
            movie7.Actors.Add(context.Actors.Find(3)); // 7. film için 3. aktörü ekliyorum
            movie7.Actors.Add(context.Actors.Find(4)); // 7. film için 4. aktörü ekliyorum
            movie7.Actors.Add(context.Actors.Find(5)); // 7. film için 5. aktörü ekliyorum

            var movie8 = context.Movies.Find(8)!;
            movie8.Actors.Add(context.Actors.Find(5)); // 8. film için 5. aktörü ekliyorum
            movie8.Actors.Add(context.Actors.Find(6)); // 8. film için 6. aktörü ekliyorum
            movie8.Actors.Add(context.Actors.Find(7)); // 8. film için 7. aktörü ekliyorum

            var movie9 = context.Movies.Find(9)!;
            movie9.Actors.Add(context.Actors.Find(7)); // 9. film için 7. aktörü ekliyorum
            movie9.Actors.Add(context.Actors.Find(8)); // 9. film için 8. aktörü ekliyorum
            movie9.Actors.Add(context.Actors.Find(9)); // 9. film için 9. aktörü ekliyorum

            var movie10 = context.Movies.Find(10)!;
            movie10.Actors.Add(context.Actors.Find(9)); // 10. film için 9. aktörü ekliyorum
            movie10.Actors.Add(context.Actors.Find(10)); // 10. film için 10. aktörü ekliyorum
            movie10.Actors.Add(context.Actors.Find(1)); // 10. film için 1. aktörü ekliyorum
        }
    }
}