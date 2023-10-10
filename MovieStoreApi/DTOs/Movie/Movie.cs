namespace MovieStoreApi.DTOs;

public class MovieResponse
{
    public int Id {get;set;}
    public string Name { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public virtual List<string> Genres { get; set; }
    public virtual List<MovieActorResponse> Actors { get; set; }
    public virtual MovieDirectorResponse Director { get; set; }
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
