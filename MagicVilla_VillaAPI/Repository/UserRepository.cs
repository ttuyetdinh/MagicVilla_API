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
using Microsoft.EntityFrameworkCore;
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

        public async Task<TokenDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequest.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (user == null || isValid == false) return new TokenDTO
            {
                AccessToken = "",
                RefreshToken = ""
            };

            // create new JTI token each success login request
            var jwtTokenId = $"JTI{Guid.NewGuid()}";

            return new TokenDTO()
            {
                AccessToken = await GetAccessToken(user, jwtTokenId),
                RefreshToken = CreateNewRefreshToken(user.Id, jwtTokenId)
            };
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
                    if (!await _roleManager.RoleExistsAsync(GetRole(registerationRequestDTO.Role)))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(GetRole(registerationRequestDTO.Role)));
                    }
                    await _userManager.AddToRoleAsync(newUser, GetRole(registerationRequestDTO.Role));
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(newUser);
                }
            }
            catch (Exception e) { }
            return new UserDTO();
        }

        public async Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO)
        {
            // Find an existing refresh token
            var existingRefreshToken = _db.RefreshTokens.FirstOrDefault(u => u.Refresh_Token == tokenDTO.RefreshToken);
            if (existingRefreshToken == null) return new TokenDTO();

            // Compare reveived data with existing refresh,access token provided. If there is mismatch => fraud
            bool isTokenValid = GetAccessTokenData(tokenDTO.AccessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            // Check if a case tried to use invalid refresh token => fraud
            if (!existingRefreshToken.IsValid.Value)
            {
                await MarkAllTokensAsInValid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
                return new TokenDTO();
            }

            // Check if a refresh token is expired => set invalid, return empty
            if (existingRefreshToken.ExpriesAt < DateTime.Now)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenDTO();
            }

            // Replace old refresh token with new refresh token with updated expired date
            var newRefreshToken = CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            // Revoke old refresh token
            await MarkTokenAsInvalid(existingRefreshToken);

            // Generate new access token
            var applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == existingRefreshToken.UserId);
            if (applicationUser == null) return new TokenDTO();

            var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);

            return new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


        // =================== ultilities =================
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

        private async Task<string> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {
            // generate JWT token if user is found
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string CreateNewRefreshToken(string userId, string tokenId)
        {
            var refreshToken = new RefreshToken()
            {
                UserId = userId,
                JwtTokenId = tokenId,
                IsValid = true,
                ExpriesAt = DateTime.Now.AddMinutes(2),
                Refresh_Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
            };

            _db.RefreshTokens.Add(refreshToken);
            _db.SaveChangesAsync();

            return refreshToken.Refresh_Token;
        }

        private bool GetAccessTokenData(string accessToken, string expectedUserId, string expectedTokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Jti).Value;
                var userId = jwt.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Sub).Value;

                return userId == expectedTokenId && jwtTokenId == expectedTokenId;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Mark all the refresh tokens in the chain as invalid
        private async Task MarkAllTokensAsInValid(string userId, string tokenId)
        {
            var chainRecords = _db.RefreshTokens
                                .Where(u => u.UserId == userId && u.JwtTokenId == tokenId)
                                .ExecuteUpdateAsync(u => u.SetProperty(token => token.IsValid, false));
        }

        private async Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            await _db.SaveChangesAsync();
        }
    }
}