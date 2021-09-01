using Assignment3.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Controllers
{
    public class CharactersController : ControllerBase
    {
        private readonly MovieDbContext Context;
        private readonly IMapper Mapper;

        public CharactersController(MovieDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
    }
}
