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
                .ForMember(mdto => mdto.Franchise, opt => opt
                .MapFrom(m => m.FranchiseId))
                // Turning related characters into arrays
                .ForMember(mdto => mdto.Characters, opt => opt
                .MapFrom(m => m.Characters.Select(c => c.Id).ToArray()))
                .ReverseMap();

            CreateMap<Movie, MovieEditDTO>()
                .ForMember(mdto => mdto.Franchise, opt => opt
                .MapFrom(m => m.FranchiseId))
                .ReverseMap();

            CreateMap<Movie, MovieCreateDTO>()
                .ForMember(mdto => mdto.Franchise, opt => opt
                .MapFrom(m => m.FranchiseId))
                .ReverseMap();
        }
    }
}
