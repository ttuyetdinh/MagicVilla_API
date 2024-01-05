using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/UsersAuth")]
    public class UserController : Controller
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IUserRepository _dbUser;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public UserController(ILogger<VillaAPIController> logger, IUserRepository dbUser, IMapper mapper)
        {
            _logger = logger;
            _dbUser = dbUser;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _dbUser.Login(model);

            if (loginResponse.localUser == null || loginResponse.Token == "")
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessage = new List<string>() { "Username or password is incorrect" }
                });
            }

            return Ok(new APIResponse
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = loginResponse
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool isUnique = _dbUser.IsUniqueUser(model.UserName);

            if (!isUnique) return BadRequest(new APIResponse
            {
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = new List<string>() { "Username already exist" }
            });

            var user = await _dbUser.Register(model);

            if(user == null) return BadRequest(new APIResponse
            {
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessage = new List<string>() { "Error while registering" }
            });


            return Ok(new APIResponse
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            });
        }
    }
}