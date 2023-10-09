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
            CreateMap<Actor, ActorResponse>();
            CreateMap<Director, DirectorResponse>();
            CreateMap<Genre, GenreResponse>();
        }
    }
}