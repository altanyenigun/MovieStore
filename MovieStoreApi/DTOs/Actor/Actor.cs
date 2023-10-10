namespace MovieStoreApi.DTOs;

public class ActorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual List<ActorMovieResponse> Movies { get; set; }
}

public class ActorCreateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class ActorUpdateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class ActorMovieResponse
{
    public string Name { get; set; }
    public int Year { get; set; }
}

