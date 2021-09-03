using Assignment3.Models;
using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Character;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Controllers
{
    /// <summary>
    /// Controller class that handles characters endpoint logic
    /// </summary>
    [Route("api/v1/characters")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CharactersController : ControllerBase
    {
        private readonly MovieDbContext Context;
        private readonly IMapper Mapper;

        public CharactersController(MovieDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        /// <summary>
        /// Gets all the characters in the database.
        /// </summary>
        /// <returns>A Collection of Character objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacters()
        {
            return Mapper.Map<List<CharacterReadDTO>>(await Context.Characters.ToListAsync());
        }

        /// <summary>
        /// Gets a secific character by it's Id
        /// </summary>
        /// <param name="id">Character's id</param>
        /// <returns>A Character object</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterReadDTO>> GetCharacter(int id)
        {
            var character = await Context.Characters
                .Include(f => f.Movies)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (character == null)
                return NotFound();

            return Mapper.Map<CharacterReadDTO>(character);
        }

        /// <summary>
        /// Edit a character object.
        /// </summary>
        /// <param name="id">Character's id</param>
        /// <param name="character">Character's new values</param>
        /// <returns>No content</returns>
        /// <exception cref="DbUpdateConcurrencyException">Exeption</exception>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, CharacterEditDTO character)
        {
            if (id != character.Id)
                return BadRequest();
            Character domainCharacter = Mapper.Map<Character>(character);
            Context.Entry(domainCharacter).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        /// <summary>
        /// Creates  a new character to the database.
        /// </summary>
        /// <param name="dtoCharacter">Values to the character object.</param>
        /// <returns>Just created character object.</returns>
        [HttpPost]
        public async Task<ActionResult<CharacterCreateDTO>> PostCharacter(CharacterCreateDTO dtoCharacter)
        {
            Character domainCharacter = Mapper.Map<Character>(dtoCharacter);
            Context.Characters.Add(domainCharacter);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter",
                new { id = domainCharacter.Id },
                Mapper.Map<CharacterReadDTO>(domainCharacter));
        }

        /// <summary>
        /// Deleting an existing character.
        /// </summary>
        /// <param name="id">Id of the deleted character</param>
        /// <returns>No content</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var character = await Context.Characters.FindAsync(id);

            if (character == null) return NotFound();

            Context.Characters.Remove(character);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks that the character exists
        /// </summary>
        /// <param name="id">Characters id</param>
        /// <returns>Bool value, if the character exists</returns>
        private bool CharacterExists(int id)
        {
            return Context.Characters.Any(e => e.Id == id);
        }
    }

}
