using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Movie;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieReadDTO>()
                // Turning related characters into arrays
                .ForMember(mdto => mdto.Characters, opt => opt
                .MapFrom(m => m.Characters.Select(c => c.Id).ToArray()))
                .ReverseMap();

            CreateMap<Movie, MovieEditDTO>()
                .ReverseMap();

            CreateMap<Movie, MovieCreateDTO>()
                .ReverseMap();
        }
    }
}
