using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Franchise;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Profiles
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseReadDTO>()
                // Turning related movies into arrays
                .ForMember(fdto => fdto.Movies, opt => opt
                .MapFrom(f => f.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();

            CreateMap<Franchise, FranchiseEditDTO>().ReverseMap();

            CreateMap<Franchise, FranchiseCreateDTO>().ReverseMap();
        }
    }
}
