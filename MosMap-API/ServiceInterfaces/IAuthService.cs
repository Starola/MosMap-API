using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MosMap_API.Dtos;
using MosMap_API.Models;

namespace MosMap_API.ServiceInterfaces
{
    public interface IAuthService
    {
        Task<User> Register(UserForRegisterDto userForRegisterDto);
        Task<SecurityToken> Login(UserForLoginDto userForLoginDto);
        Task<bool> UserExists(string username);
    }
}