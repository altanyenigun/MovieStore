using MovieStoreApi.Models;

namespace MovieStoreApi.DTOs;

public class DirectorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public virtual List<MovieResponse> Movies { get; set; }
}
