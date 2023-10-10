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
            CreateMap<Actor, MovieActorResponse>();
            CreateMap<Director, MovieDirectorResponse>();
            CreateMap<Genre, MovieGenreResponse>();

            CreateMap<Actor, ActorResponse>();
            CreateMap<Movie, ActorMovieResponse>();
            CreateMap<Movie, DirectorMovieResponse>();
            CreateMap<MovieCreateRequest, Movie>();
            CreateMap<MovieUpdateRequest, Movie>();



            CreateMap<Director, DirectorResponse>();

            CreateMap<ActorUpdateRequest, Actor>();
            CreateMap<ActorCreateRequest, Actor>();

            CreateMap<DirectorUpdateRequest, Director>();
            CreateMap<DirectorCreateRequest, Director>();




        }
    }
}