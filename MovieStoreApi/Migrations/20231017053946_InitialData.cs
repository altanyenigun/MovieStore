using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStoreApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminPassword = "admin"; // Kullanıcının gerçek şifresi
            var hashedAdminPassword = BCrypt.Net.BCrypt.HashPassword(adminPassword);

            var customerPassword = "customer";
            var hashedCustomerPassword = BCrypt.Net.BCrypt.HashPassword(customerPassword);

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Directors_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Directors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGenre",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGenre", x => new { x.CustomersId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_CustomerGenre_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActorMovie",
                columns: table => new
                {
                    ActorsId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovie", x => new { x.ActorsId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_ActorMovie_Actors_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMovie",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMovie", x => new { x.CustomersId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_CustomerMovie_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovie",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovie", x => new { x.GenresId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_GenreMovie_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovie_MoviesId",
                table: "ActorMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGenre_GenresId",
                table: "CustomerGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMovie_MoviesId",
                table: "CustomerMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MoviesId",
                table: "GenreMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_DirectorId",
                table: "Movies",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MovieId",
                table: "Orders",
                column: "MovieId");

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Name", "Surname" },
                values: new object[,]
                {
                    { "Jane", "Smith" },
                    { "Michael", "Johnson" },
                    { "Emily", "Brown" },
                    { "James", "Taylor" },
                    { "Olivia", "Anderson" },
                    { "Daniel", "Martinez" },
                    { "Sophia", "Wilson" },
                    { "David", "Lee" },
                    { "Emma", "Lopez" },
                    { "Liam", "Garcia" }
            });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Username", "Name", "Surname", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { "admin", "admin", "admin", hashedAdminPassword, "Admin" },
                    { "altan", "altan", "yen", hashedCustomerPassword, "Customer" },
                    { "emilyb", "Emily", "Brown", hashedCustomerPassword, "Customer" },
                    { "jamest", "James", "Taylor", hashedCustomerPassword, "Customer" },
                    { "oliviaa", "Olivia", "Anderson", hashedCustomerPassword, "Customer" },
                    { "danielm", "Daniel", "Martinez", hashedCustomerPassword, "Customer" },
                    { "sophiaw", "Sophia", "Wilson", hashedCustomerPassword, "Customer" },
                    { "davidl", "David", "Lee", hashedCustomerPassword, "Customer" },
                    { "emmal", "Emma", "Lopez", hashedCustomerPassword, "Customer" },
                    { "liamg", "Liam", "Garcia", hashedCustomerPassword, "Customer" }
            });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Name", "Surname" },
                values: new object[,]
                {
                    { "Christopher", "Smith" },
                    { "Jessica", "Davis" },
                    { "William", "Harris" },
                    { "Natalie", "Thomas" },
                    { "Robert", "Brown" },
                    { "Ava", "Evans" },
                    { "Matthew", "Perez" },
                    { "Olivia", "Rivera" },
                    { "James", "Long" },
                    { "Emma", "Bell" }
            });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Drama" },
                    { "Thriller" },
                    { "Science Fiction" },
                    { "Romance" },
                    { "Horror" },
                    { "Mystery" },
                    { "Adventure" },
                    { "Fantasy" },
                    { "Comedy" },
                    { "Animation" }
            });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Name", "Year", "Price", "isActive", "DirectorId" },
                values: new object[,]
                {
                    { "Movie1", 2020, 9.99, true, 1 },
                    { "Movie2", 2019, 12.99, true, 2 },
                    { "Movie3", 2021, 14.99, true, 3 },
                    { "Movie4", 2018, 11.99, true, 4 },
                    { "Movie5", 2017, 10.99, true, 5 },
                    { "Movie6", 2022, 16.99, true, 6 },
                    { "Movie7", 2016, 13.99, true, 7 },
                    { "Movie8", 2015, 17.99, true, 8 },
                    { "Movie9", 2023, 19.99, true, 9 },
                    { "Movie10", 2014, 15.99, true, 10 }
            });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "CustomerId", "MovieId", "Price", "PurchaseDate" },
                values: new object[,]
                {
                    { 1, 1, 9.99, new DateTime(2023, 10, 17) },
                    { 1, 2, 12.99, new DateTime(2023, 10, 18) },
                    { 1, 3, 14.99, new DateTime(2023, 10, 19) },
                    { 2, 3, 14.99, new DateTime(2023, 10, 20) },
                    { 2, 4, 11.99, new DateTime(2023, 10, 21) },
                    { 2, 5, 10.99, new DateTime(2023, 10, 22) },
                    { 3, 5, 10.99, new DateTime(2023, 10, 23) },
                    { 3, 6, 16.99, new DateTime(2023, 10, 24) },
                    { 3, 7, 13.99, new DateTime(2023, 10, 25) },
                    { 4, 7, 13.99, new DateTime(2023, 10, 26) },
                    { 4, 8, 17.99, new DateTime(2023, 10, 27) },
                    { 4, 9, 19.99, new DateTime(2023, 10, 28) },
                    { 5, 9, 19.99, new DateTime(2023, 10, 29) },
                    { 5, 10, 15.99, new DateTime(2023, 10, 30) },
                    { 5, 1, 9.99, new DateTime(2023, 10, 31) },
                    { 6, 1, 9.99, new DateTime(2023, 11, 1) },
                    { 6, 2, 12.99, new DateTime(2023, 11, 2) },
                    { 6, 3, 14.99, new DateTime(2023, 11, 3) },
                    { 7, 3, 14.99, new DateTime(2023, 11, 4) },
                    { 7, 4, 11.99, new DateTime(2023, 11, 5) },
                    { 7, 5, 10.99, new DateTime(2023, 11, 6) },
                    { 8, 5, 10.99, new DateTime(2023, 11, 7) },
                    { 8, 6, 16.99, new DateTime(2023, 11, 8) },
                    { 8, 7, 13.99, new DateTime(2023, 11, 9) },
                    { 9, 7, 13.99, new DateTime(2023, 11, 10) },
                    { 9, 8, 17.99, new DateTime(2023, 11, 11) },
                    { 9, 9, 19.99, new DateTime(2023, 11, 12) },
                    { 10, 9, 19.99, new DateTime(2023, 11, 13) },
                    { 10, 10, 15.99, new DateTime(2023, 11, 14) },
                    { 10, 1, 9.99, new DateTime(2023, 11, 15) }
            });

            migrationBuilder.InsertData(
                table: "GenreMovie",
                columns: new[] { "GenresId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 7, 7 },
                    { 7, 8 },
                    { 7, 9 },
                    { 8, 8 },
                    { 8, 9 },
                    { 8, 10 },
                    { 9, 9 },
                    { 9, 10 },
                    { 9, 1 },
                    { 10, 10 },
                    { 10, 1 },
                    { 10, 2 }
            });

            migrationBuilder.InsertData(
                table: "ActorMovie",
                columns: new[] { "ActorsId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 7 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 5, 9 },
                    { 5, 10 },
                    { 5, 1 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 9, 7 },
                    { 9, 8 },
                    { 9, 9 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 1 }
            });

            migrationBuilder.InsertData(
                table: "CustomerGenre",
                columns: new[] { "CustomersId", "GenresId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 7, 7 },
                    { 7, 8 },
                    { 7, 9 },
                    { 8, 8 },
                    { 8, 9 },
                    { 8, 10 },
                    { 9, 9 },
                    { 9, 10 },
                    { 9, 1 },
                    { 10, 10 },
                    { 10, 1 },
                    { 10, 2 }
            });

            migrationBuilder.InsertData(
                table: "CustomerMovie",
                columns: new[] { "CustomersId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 7 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 5, 9 },
                    { 5, 10 },
                    { 5, 1 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 9, 7 },
                    { 9, 8 },
                    { 9, 9 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 1 }
            });
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorMovie");

            migrationBuilder.DropTable(
                name: "CustomerGenre");

            migrationBuilder.DropTable(
                name: "CustomerMovie");

            migrationBuilder.DropTable(
                name: "GenreMovie");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValues: new object[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            
            migrationBuilder.DeleteData(
                table: "GenreMovie",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 7, 7 },
                    { 7, 8 },
                    { 7, 9 },
                    { 8, 8 },
                    { 8, 9 },
                    { 8, 10 },
                    { 9, 9 },
                    { 9, 10 },
                    { 9, 1 },
                    { 10, 10 },
                    { 10, 1 },
                    { 10, 2 }
            });

            migrationBuilder.DeleteData(
                table: "ActorMovie",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 7 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 5, 9 },
                    { 5, 10 },
                    { 5, 1 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 9, 7 },
                    { 9, 8 },
                    { 9, 9 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 1 }
            });

            migrationBuilder.DeleteData(
                table: "CustomerGenre",
                keyColumns: new[] { "CustomersId", "GenresId" },
                keyValues: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 },
                    { 6, 6 },
                    { 6, 7 },
                    { 6, 8 },
                    { 7, 7 },
                    { 7, 8 },
                    { 7, 9 },
                    { 8, 8 },
                    { 8, 9 },
                    { 8, 10 },
                    { 9, 9 },
                    { 9, 10 },
                    { 9, 1 },
                    { 10, 10 },
                    { 10, 1 },
                    { 10, 2 }
            });

            migrationBuilder.DeleteData(
                table: "CustomerMovie",
                keyColumns: new[] { "CustomersId", "MoviesId" },
                keyValues: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 3, 5 },
                    { 3, 6 },
                    { 3, 7 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 5, 9 },
                    { 5, 10 },
                    { 5, 1 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 3 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 8, 5 },
                    { 8, 6 },
                    { 8, 7 },
                    { 9, 7 },
                    { 9, 8 },
                    { 9, 9 },
                    { 10, 9 },
                    { 10, 10 },
                    { 10, 1 }
            });
        }
    }
}
