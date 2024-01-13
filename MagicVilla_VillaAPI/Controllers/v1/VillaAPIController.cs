using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using MagicVilla_Ultility;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIController(ILogger<VillaAPIController> logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30sec",VaryByQueryKeys = new[] {"explicitOcuppancy", "search", "PageNum", "PageSize"} )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "explicitOcuppancy")] int? ocuppancy, [FromQuery] string? search, [FromQuery] Pagination? pagination)
        {
            try
            {
                // filter in db
                var villaList = ocuppancy == null ? await _dbVilla.GetAllAsync(pagination: pagination) : await _dbVilla.GetAllAsync(u => u.Occupancy == ocuppancy, pagination: pagination);

                // filter the result get from
                villaList = search != null ? villaList.Where(u => u.Name.ToLower().Contains(search)).ToList() : villaList;

                Response.Headers.Add("X-pagination", JsonSerializer.Serialize(pagination));

                _logger.LogInformation($"Finished retrieving {villaList.Count} villa");

                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var villa = await _dbVilla.GetAsync(i => i.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;


                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;

        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpGet("{name}", Name = "GetVillaName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaWithName(string name)
        {
            try
            {
                if (name == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var villa = await _dbVilla.GetAsync(i => i.Name.ToLower().Contains(name.ToLower()));

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;


                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createVillaDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(i => i.Name == createVillaDTO.Name) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa name is already exists");

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Result = ModelState;

                    return BadRequest(_response);
                }
                // somehow this if never reached
                if (createVillaDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                    // throw new ArgumentNullException(nameof(createVillaDTO));
                }

                Villa villaData = _mapper.Map<Villa>(createVillaDTO);

                await _dbVilla.CreateAsync(villaData);

                _response.Result = _mapper.Map<VillaDTO>(villaData);
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetVilla", new { id = villaData.Id }, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var villa = await _dbVilla.GetAsync(i => i.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_response);
                }

                await _dbVilla.RemoveAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateVillaDTO)
        {
            try
            {
                if (updateVillaDTO == null || id != updateVillaDTO.Id)
                {
                    return BadRequest();
                }

                Villa villaData = await _dbVilla.GetAsync(i => i.Id == id);
                villaData = _mapper.Map(updateVillaDTO, villaData);

                await _dbVilla.UpdateAsync(villaData);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        [HttpPatch("{id:int}", Name = "UpdatePatchVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    return BadRequest();
                }

                var villa = await _dbVilla.GetAsync(i => i.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                var updateVillaDTO = _mapper.Map<VillaUpdateDTO>(villa);

                patchDTO.ApplyTo(updateVillaDTO, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                villa = _mapper.Map(updateVillaDTO, villa);

                await _dbVilla.UpdateAsync(villa);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>(){
                    e.ToString()
                };
            }

            return _response;
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