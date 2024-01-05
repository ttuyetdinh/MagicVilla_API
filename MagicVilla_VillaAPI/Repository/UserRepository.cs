using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            // secretKey = configuration["APISettings:SecretKey"]; // both lines are the same
            secretKey = configuration.GetValue<string>("APISettings:SecretKey");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName == username);

            return user == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName == loginRequest.UserName && u.Password == loginRequest.Password);
            if (user == null) return new LoginResponseDTO{
                Token = "",
                localUser = null
            };

            // generate JWT token if user is found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
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
                localUser = user
            };

            return loginResponseDTO;

        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            var newUser = new LocalUser()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role
            };

            _db.LocalUsers.Add(newUser);
            await _db.SaveChangesAsync();

            newUser.Password = "";
            return newUser;
        }
    }
}