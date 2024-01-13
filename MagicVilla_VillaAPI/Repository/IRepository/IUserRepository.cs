using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}