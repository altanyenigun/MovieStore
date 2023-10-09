namespace MovieStoreApi.DTOs
{
    public class MovieResponse
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public virtual List<string> Genres { get; set; }
        public virtual List<ActorResponse> Actors { get; set; }
        public virtual DirectorResponse Director { get; set; }
    }
}