using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Controllers.v1;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiVersionNeutral]
    public class UserController : Controller
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public UserController(ILogger<VillaAPIController> logger, IUserRepository userRepo, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error (){
            throw new FileNotFoundException();
        }

        [HttpGet("imageError")]
        public async Task<IActionResult> ImageError (){
            throw new BadImageFormatException("Fake image exception"); 
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var tokenDto = await _userRepo.Login(model);

            if (tokenDto == null || tokenDto.AccessToken == "")
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = new List<string>() { "Username or password is incorrect" }
                });
            }

            return Ok(new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = tokenDto
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO model)
        {
            bool isUnique = _userRepo.IsUniqueUser(model.UserName);

            if (!isUnique) return BadRequest(new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = new List<string>() { "Username already exist" }
            });

            var user = await _userRepo.Register(model);

            if (user == null) return BadRequest(new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = new List<string>() { "Error while registering" }
            });


            return Ok(new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            });
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNewTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                var tokenDTOResponse = await _userRepo.RefreshAccessToken(tokenDTO);
                if (tokenDTOResponse == null || string.IsNullOrEmpty(tokenDTOResponse.AccessToken))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessage = new List<string>() { "Token invalid" };

                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = tokenDTOResponse;

                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Result = "Invalid input";
                return BadRequest(_response);
            }
        }

        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokerefreshToken([FromBody] TokenDTO tokenDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = "Invalid input"
                });
            };

            await _userRepo.RevokeRefreshToken(tokenDTO);
            return Ok(new APIResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK
            });

        }
    }
}