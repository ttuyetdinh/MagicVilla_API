using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ILogger<VillaNumberAPIController> _logger;
        private readonly IVillaNumberRepository _dbVillaNumber;

        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(ILogger<VillaNumberAPIController> logger, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var includeProperties = "Villa";
                var villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:includeProperties);

                _logger.LogInformation($"Finished retrieving {villaNumberList.Count} villa");

                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                var includeProperties = "Villa";
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var villa = await _dbVillaNumber.GetAsync(filter: i => i.VillaNo == id, includeProperties: includeProperties);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDTO>(villa);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (villaNumberDTO.VillaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>(){
                        $"villa Number can be 0"
                    };

                    return BadRequest(_response);
                }

                if (await _dbVillaNumber.GetAsync(i => i.VillaNo == villaNumberDTO.VillaNo && i.VillaId == villaNumberDTO.VillaId) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Number is already exist for this Villa"
                    };
                    return BadRequest(_response);
                }

                if (await _dbVilla.GetAsync(i => i.Id == villaNumberDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Id is not exist"
                    };
                    return BadRequest(_response);
                }

                var villaNumberData = _mapper.Map<VillaNumber>(villaNumberDTO);

                await _dbVillaNumber.CreateAsync(villaNumberData);

                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumberData);
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetVillaNumber", new { id = villaNumberData.VillaNo }, _response);
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

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var villa = await _dbVillaNumber.GetAsync(i => i.VillaNo == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_response);
                }

                await _dbVillaNumber.RemoveAsync(villa);

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

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateVillaNumberDTO)
        {
            try
            {
                if (updateVillaNumberDTO == null || id != updateVillaNumberDTO.VillaNo)
                {
                    return BadRequest();
                }

                var villa = await _dbVillaNumber.GetAsync(i => i.VillaNo == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Number is not exsit"
                    };

                    return NotFound(_response);
                }

                if (await _dbVilla.GetAsync(i => i.Id == updateVillaNumberDTO.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Id is not exsit"
                    };
                    return BadRequest(_response);
                }

                villa = _mapper.Map(updateVillaNumberDTO, villa);

                await _dbVillaNumber.UpdateAsync(villa);

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

        [HttpPatch("{id:int}", Name = "UpdatePatchVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> UpdatePatchVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _dbVillaNumber.GetAsync(i => i.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Number is not exsit"
                    };

                    return NotFound(_response);
                }


                var updateVillaDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

                patchDTO.ApplyTo(updateVillaDTO, ModelState);
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                villaNumber = _mapper.Map(updateVillaDTO, villaNumber);
                
                if (await _dbVilla.GetAsync(i => i.Id == villaNumber.VillaId) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>(){
                        $"Villa Id is not exsit"
                    };
                    return BadRequest(_response);
                }

                await _dbVillaNumber.UpdateAsync(villaNumber);

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