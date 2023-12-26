using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;


        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var villaList = await _db.Villas.ToListAsync();
            _logger.LogInformation($"Finished retrieving {villaList.Count} villa");
            return Ok(villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _db.Villas.FirstOrDefaultAsync(i => i.Id == id);
            if (villa == null) return NotFound();
            return Ok(villa);
        }

        [HttpGet("{name}", Name = "GetVillaName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaWithName(string name)
        {
            if (name == null) return BadRequest();
            var villa = await _db.Villas.FirstOrDefaultAsync(i => i.Name.ToLower().Contains(name.ToLower()));
            if (villa == null) return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if (await _db.Villas.FirstOrDefaultAsync(i => i.Name == villaDTO.Name) != null)
            {
                ModelState.AddModelError("CustomError", "Villa name is already exists");
                return BadRequest(villaDTO);
            }
            if (villaDTO == null)
            {
                return BadRequest();
                // throw new ArgumentNullException(nameof(villaDTO));
            }


            Villa data = new()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
                Age = villaDTO.Age,
                CreatedDate = DateTime.Now
            };

            await _db.Villas.AddAsync(data);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = data.Id }, data);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.FirstOrDefaultAsync(i => i.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            // var villa =  _db.Villas.FirstOrDefault(i => i.Id == id);

            // if (villa == null)
            // {
            //     return NotFound();
            // }

            Villa data = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
                Age = villaDTO.Age,
            };

            _db.Villas.Update(data);
            await _db.SaveChangesAsync();


            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePatchVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaUpdateDTO dataDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Amenity = villa.Amenity,
                Age = villa.Age
            };

            patchDTO.ApplyTo(dataDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa data = new()
            {
                Id = dataDTO.Id,
                Name = dataDTO.Name,
                Details = dataDTO.Details,
                ImageUrl = dataDTO.ImageUrl,
                Occupancy = dataDTO.Occupancy,
                Rate = dataDTO.Rate,
                Sqft = dataDTO.Sqft,
                Amenity = dataDTO.Amenity,
                Age = dataDTO.Age
            };

            _db.Villas.Update(data);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        // ultilitiy functions
        // private int GenerateNewId()
        // {
        //     return VillaStore.villaList.Count > 0
        //         ? VillaStore.villaList.Max(i => i.Id) + 1
        //         : 1;
        // }
    }
}