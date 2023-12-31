using System.Text.Json.Serialization;
using MovieStoreApi.Models;

namespace MovieStoreApi.DTOs;

public class OrderRequest
{
    public int CustomerId { get; set; }
    public int MovieId { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class CustomerOrderResponse
{
    public string MovieName { get; set; }
    public int Price { get; set; }
    public DateTime PurchaseDate { get; set; }
}