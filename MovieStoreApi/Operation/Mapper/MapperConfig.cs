using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MovieStoreApi.DTOs;
using MovieStoreApi.Models;

namespace MovieStoreApi.Operation.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Movie, MovieResponse>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()));
            CreateMap<Movie, ActorMovieResponse>();
            CreateMap<Movie, DirectorMovieResponse>();
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<MovieUpdateRequest, Movie>();

            CreateMap<Actor, ActorResponse>();
            CreateMap<Actor, MovieActorResponse>();
            CreateMap<ActorUpdateRequest, Actor>();
            CreateMap<ActorCreateRequest, Actor>();

            CreateMap<Director, MovieDirectorResponse>();
            CreateMap<Director, DirectorResponse>();
            CreateMap<DirectorUpdateRequest, Director>();
            CreateMap<DirectorCreateRequest, Director>();

            CreateMap<Genre, MovieGenreResponse>();

            CreateMap<CustomerRegisterRequest, Customer>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<Order, CustomerOrderResponse>()
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Movie.Price));

        }
    }
}