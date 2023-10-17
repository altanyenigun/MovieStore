using System.Text.Json.Serialization;
using MovieStoreApi.Models;

namespace MovieStoreApi.DTOs;

public class MovieResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public virtual List<string> Genres { get; set; }
    public virtual List<MovieActorResponse> Actors { get; set; }
    public virtual MovieDirectorResponse Director { get; set; }
}

public class MovieCreateRequest
{
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    [JsonIgnore]
    public bool isActive { get; set; } = true;

    public List<int> GenreIds { get; set; } // Sadece Genre'ların Id'lerini alacağız
    public int? DirectorId { get; set; }     // Sadece Director'ün Id'sini alacağız
    public List<int> ActorIds { get; set; } // Sadece Actor'ların Id'lerini alacağız
}

public class MovieUpdateRequest
{
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public List<int> GenreIds { get; set; }
    public int DirectorId { get; set; }
    public List<int> ActorIds { get; set; }
}

public class MovieActorResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class MovieDirectorResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class MovieGenreResponse
{
    public string Name { get; set; }
}
