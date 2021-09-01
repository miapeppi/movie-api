using Assignment3.Models;
using Assignment3.Models.Domain;
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
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchise(int id)
        {
            var franchise = await Context.Franchises
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (franchise == null) return NotFound();

            return Mapper.Map<FranchiseReadDTO>(franchise);
        }

        /// <summary>
        /// Updates a franchise in database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="franchise"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFranchise(int id, FranchiseEditDTO franchise)
        {
            if (id != franchise.Id)
                return BadRequest();

            Franchise domainFranchise = Mapper.Map<Franchise>(franchise);
            Context.Entry(domainFranchise).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FranchiseExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }
        /// <summary>
        /// Adds a franchise to database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FranchiseCreateDTO>> PostFranchise(FranchiseCreateDTO dtoFranchise)
        {
            Franchise domainFranchise = Mapper.Map<Franchise>(dtoFranchise);
            Context.Franchises.Add(domainFranchise);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetFranchise",
                new { id = domainFranchise.Id },
                Mapper.Map<FranchiseReadDTO>(domainFranchise));
        }

        /// <summary>
        /// Deletes a franchise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            var franchise = await Context.Franchises.FindAsync(id);

            if (franchise == null) return NotFound();

            Context.Franchises.Remove(franchise);
            await Context.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Updates Movies in a Franchise by movie IDs.
        /// </summary>
        /// <param name="id">ID of Franchise to update</param>
        /// <param name="movieIds">Array of Movie IDs to update in Franchise</param>
        /// <returns></returns>
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutFranchiseMovies(int id, int[] movieIds)
        //{
        //    //Get Franchise from database
        //    var franchise = await Context.Franchises
        //        .Include(f => f.Movies)
        //        .FirstOrDefaultAsync(f => f.Id == id);

        //    //Check if franchise exists
        //    if (franchise == null) return NotFound();

        //    //Map franchise to DTO
        //    FranchiseReadDTO franchiseDTO = Mapper.Map<FranchiseReadDTO>(franchise);

        //    //Check if movie IDs exist in database


        //    //Context.Entry(domainFranchise).State = EntityState.Modified;

        //    try
        //    {
        //        await Context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FranchiseExists(id))
        //            return NotFound();
        //        else
        //            throw;
        //    }
        //    return NoContent();
        //}

        private bool FranchiseExists(int id)
        {
            return Context.Franchises.Any(e => e.Id == id);
        }
    }
}
