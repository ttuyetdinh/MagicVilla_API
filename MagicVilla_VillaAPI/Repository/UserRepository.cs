using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Ultility;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using static MagicVilla_Ultility.SD;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly string secretKey;

        public UserRepository(ApplicationDbContext db,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            // secretKey = configuration["APISettings:SecretKey"]; // both lines are the same
            secretKey = configuration.GetValue<string>("APISettings:SecretKey");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);

            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequest.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (user == null || isValid == false) return new LoginResponseDTO
            {
                Token = "",
                ApplicationUser = null
            };

            // generate JWT token if user is found
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                ApplicationUser = _mapper.Map<UserDTO>(user),
                // Role = roles.FirstOrDefault()
            };

            return loginResponseDTO;

        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var newUser = new ApplicationUser()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName,
                Name = registerationRequestDTO.Name

            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if(! await _roleManager.RoleExistsAsync(GetRole(registerationRequestDTO.Role))   ){
                        await _roleManager.CreateAsync(new IdentityRole(GetRole(registerationRequestDTO.Role)));
                    }
                    await _userManager.AddToRoleAsync(newUser, GetRole(registerationRequestDTO.Role));
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(newUser);

                }
            }
            catch (Exception e)
            {

            }

            return new UserDTO();
        }

        // ultilities
        private string GetRole(Role? role)
        {
            return role switch
            {
                Role.Admin => Role.Admin.ToString(),
                Role.User => Role.User.ToString(),
                Role.CustomRole => Role.CustomRole.ToString(),
                _ => Role.Admin.ToString()
            };
        }
    }
}