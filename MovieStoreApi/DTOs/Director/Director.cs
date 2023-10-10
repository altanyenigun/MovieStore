namespace MovieStoreApi.DTOs;

public class DirectorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual List<DirectorMovieResponse> Movies { get; set; }
}

public class DirectorCreateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class DirectorUpdateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class DirectorMovieResponse
{
    public string Name { get; set; }
    public int Year { get; set; }
}
