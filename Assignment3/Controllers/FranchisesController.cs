using Assignment3.Models;
using Assignment3.Models.DTO.Franchise;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Assignment3.Controllers
{
    [Route("api/v1/franchises")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]

    public class FranchisesController : ControllerBase
    {
        private readonly MovieDbContext Context;
        private readonly IMapper Mapper;

        public FranchisesController(MovieDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchises()
        {
            return Mapper.Map<List<FranchiseReadDTO>>(await Context.Franchises.ToListAsync());
        }

        /// <summary>
        /// Gets a specific franchise by its Id
        /// </summary>
        /// <param name="id">Franchise's id value as int</param>
        /// <returns>Franchise object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetFranchise(int id)
        {
            var franchise = await Context.Franchises.FindAsync(id);

            if (franchise == null) return NotFound();

            return Mapper.Map<List<FranchiseReadDTO>>(franchise);
        }

    }
}
