using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public AuthService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }
        /*
         * Methods for register process
         */
        public async Task<User> Register(UserForRegisterDto userForRegisterDto)
        {
            //convert the username to lowercase (not possible Matt und matt)
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            
            //checks if user already exists
            if (await UserExists(userForRegisterDto.Username))
                return null;


            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };
            var createdUser = await CreateUserToRegister(userToCreate, userForRegisterDto.Password);

            return createdUser;
        }
        private async Task<User> CreateUserToRegister(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
        
        /*
         * Methods for login process
         */

        public async Task<SecurityToken> Login(UserForLoginDto userForLoginDto)
        {
            //Gets the user if the credentials are right
            var userFromRepo = await GetUserForLogin(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (userFromRepo == null)
                return null;
            //Creates token for User
            var token = CreateToken(userFromRepo);
            return token;
        }
        
        private async Task<User> GetUserForLogin(string username, string password)
        {
            //returns User with given username
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            //checks if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }
        

        private SecurityToken CreateToken(User user)
        {
            //The information in claim in the token can be accessed 
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            //server can check if the key is valid with key - this is encoded
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
        
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }
        
        /*
        * Checks if user with the same username exists
        */
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
        
    }
}