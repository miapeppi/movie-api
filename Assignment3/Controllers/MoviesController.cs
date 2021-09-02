using Assignment3.Models;
using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Character;
using Assignment3.Models.DTO.Movie;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Assignment3.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class MoviesController : ControllerBase
    {
        private readonly MovieDbContext Context;
        private readonly IMapper Mapper;
        public MoviesController(MovieDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        /// <summary>
        /// List of all movies.
        /// </summary>
        /// <returns>List of movie objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetFranchises()
        {
            return Mapper.Map<List<MovieReadDTO>>(await Context.Movies
                .Include(m => m.Characters)
                .ToListAsync());
        }
        /// <summary>
        /// Return movie by id.
        /// </summary>
        /// <param name="id">Id of movie to return.</param>
        /// <returns>Movie object</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReadDTO>> GetMovie(int id)
        {
            var movie = await Context.Movies
                .Include(f => f.Characters)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (movie == null) return NotFound();

            return Mapper.Map<MovieReadDTO>(movie);
        }
        /// <summary>
        /// Return all characters in movie by id.
        /// </summary>
        /// <param name="id">Id of movie to return characters from.</param>
        /// <returns>Collection of character objects</returns>
       [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacterMovie(int id)
        {
            var movie = await Context.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            MovieReadDTO movieDTO = Mapper.Map<MovieReadDTO>(movie);
            var characters = Context.Characters.Where(c => movieDTO.Characters.Contains(c.Id));

            return Mapper.Map<List<CharacterReadDTO>>(characters);
        }

        /// <summary>
        /// Update movie in database.
        /// </summary>
        /// <param name="id">Id of movie to update.</param>
        /// <returns>No content</returns>
        /// <Exception>DbUpdateConcurrencyException</Exception>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieEditDTO movie)
        {
            if (id != movie.Id)
                return BadRequest();
            Movie domainMovie = Mapper.Map<Movie>(movie);
            Context.Entry(domainMovie).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Add new movie to database
        /// </summary>
        /// <param name="dtoMovie">Movie object to update in database.</param>
        /// <returns>No content</returns>
        /// <Exception>DbUpdateConcurrencyException</Exception>
        [HttpPost]
        public async Task<ActionResult<MovieCreateDTO>> PostMovie(MovieCreateDTO dtoMovie)
        {
            Movie domainMovie = Mapper.Map<Movie>(dtoMovie);
            Context.Movies.Add(domainMovie);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetMovie",
                new { id = domainMovie.Id },
                Mapper.Map<MovieReadDTO>(domainMovie));
        }
        /// <summary>
        /// Add a characters to a movie.
        /// </summary>
        /// <param name="id">Movie Id to </param>
        /// <param name="characterIds">Id values of characters to add</param>
        /// <returns>Ok</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{id}/characters")]
        public async Task<ActionResult> PostCharacters(int id, int[] characterIds)
        {
            var movie = await Context.Movies
                .Include(f => f.Characters)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (movie == null) return NotFound();

            var characterIdsList = characterIds.Distinct();
            var characters = Context.Characters.Where(character => characterIdsList.Any(id => id == character.Id)).ToList();

            var missingIds = characterIds.Where(id => !characters.Any(character => character.Id == id));
            if (missingIds.Count() > 0)
            {
                return BadRequest(missingIds);
            }
            movie.Characters = characters;
            await Context.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Set all characters in a movie.
        /// </summary>
        /// <param name="id">Id of movie to set characters in.</param>
        /// <param name="characterId">Id of characters to set in movie.</param>
        /// <returns>Ok</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}/characters")]
        public async Task<ActionResult> PutCharacterMovie(int id, int[] characterId)
        {
            var movie = await Context.Movies
                .Include(m => m.Characters)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            var charactersIdList = characterId.Distinct();
            var characters = Context.Characters.Where(c => charactersIdList.Any(id => id == c.Id)).ToList();
            var missingIds = charactersIdList.Where(id => !characters.Any(characters => characters.Id == id));
            if (missingIds.Count() > 0)
                return BadRequest();
            movie.Characters = characters;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                    return NotFound();
                else
                    throw;
            }
            return Ok();
        }

        /// <summary>
        /// Delete a movie in database.
        /// </summary>
        /// <param name="id">Id of movie to delete.</param>
        /// <returns>No content</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await Context.Movies.FindAsync(id);

            if (movie == null) return NotFound();

            Context.Movies.Remove(movie);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if movie exists in database.
        /// </summary>
        /// <param name="id">Id of movie to check.</param>
        /// <returns>True if movie exists in database, false otherwise.</returns>
        private bool MovieExists(int id)
        {
            return Context.Movies.Any(e => e.Id == id);
        }

    }
}