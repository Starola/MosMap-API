using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MosMap_API.Data;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        /*
         * Methods for register process
         */
        public async Task<User> Register(UserForRegisterDto userForRegisterDto)
        {
            //convert the username to lowercase (not possible Matt und matt)
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            
            //checks if user already exists
            if (await _repo.UserExists(userForRegisterDto.Username))
                return null;


            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return createdUser;
        }
        /*
         * Methods for login process
         */

        public async Task<SecurityToken> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            //checks if user has correct credentials
            if (userFromRepo == null)
                return null;
            var token = CreateToken(userFromRepo);
            return token;
        }

        private SecurityToken CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            //server can check if the key is valid with key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            //description of the token - token is only valid until the expiration date
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            //token gets created
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
        
    }
}