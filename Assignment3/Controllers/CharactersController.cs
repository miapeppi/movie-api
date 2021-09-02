using Assignment3.Models;
using Assignment3.Models.Domain;
using Assignment3.Models.DTO.Character;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Assignment3.Controllers
{
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

            [HttpGet]
            public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetCharacters()
            {
                return Mapper.Map<List<CharacterReadDTO>>(await Context.Characters.ToListAsync());
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<CharacterReadDTO>> GetCharacter(int id)
            {
                var character = await Context.Characters
                    .Include(f => f.Movies)
                    .FirstOrDefaultAsync(f => f.Id == id);
                if (character == null) return NotFound();
                return Mapper.Map<CharacterReadDTO>(character);

            }

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
            private bool CharacterExists(int id)
            {
                return Context.Characters.Any(e => e.Id == id);
            }

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

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCharacter(int id)
            {
                var character = await Context.Characters.FindAsync(id);

                if (character == null) return NotFound();

                Context.Characters.Remove(character);
                await Context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
}
