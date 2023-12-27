using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
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
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var villaList = await _dbVilla.GetAllAsync();
            _logger.LogInformation($"Finished retrieving {villaList.Count} villa");

            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = await _dbVilla.GetAsync(i => i.Id == id);

            if (villa == null) return NotFound();

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpGet("{name}", Name = "GetVillaName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaWithName(string name)
        {
            if (name == null) return BadRequest();

            var villa = await _dbVilla.GetAsync(i => i.Name.ToLower().Contains(name.ToLower()));

            if (villa == null) return NotFound();

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createVillaDTO)
        {
            if (await _dbVilla.GetAsync(i => i.Name == createVillaDTO.Name) != null)
            {
                ModelState.AddModelError("CustomError", "Villa name is already exists");
                return BadRequest(createVillaDTO);
            }
            if (createVillaDTO == null)
            {
                return BadRequest();
                // throw new ArgumentNullException(nameof(createVillaDTO));
            }

            Villa villaData = _mapper.Map<Villa>(createVillaDTO);

            await _dbVilla.CreateAsync(villaData);

            return CreatedAtRoute("GetVilla", new { id = villaData.Id }, villaData);
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

            var villa = await _dbVilla.GetAsync(i => i.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            await _dbVilla.RemoveAsync(villa);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateVillaDTO)
        {
            if (updateVillaDTO == null || id != updateVillaDTO.Id)
            {
                return BadRequest();
            }

            Villa villaData = _mapper.Map<Villa>(updateVillaDTO);

            await _dbVilla.UpdateAsync(villaData);

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

            var villa = await _dbVilla.GetAsync(i => i.Id == id, tracked: false);

            if (villa == null)
            {
                return NotFound();
            }

            VillaUpdateDTO updateVillaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patchDTO.ApplyTo(updateVillaDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa villaData = _mapper.Map<Villa>(updateVillaDTO);

            await _dbVilla.UpdateAsync(villaData);

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