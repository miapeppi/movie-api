using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Character;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterReadDTO>()
            // Turning related movies into arrays
                .ForMember(cdto => cdto.Movies, opt => opt
                .MapFrom(c => c.Movies.Select(m => m.Id).ToArray()))
                .ReverseMap();

            CreateMap<Character, CharacterEditDTO>();

            CreateMap<Character, CharacterCreateDTO>();
        }
    }
}
