﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        public async Task<ApplicationUser> Register(UserForRegisterDto userForRegisterDto)
        {
            this.CheckAndCreateAuthRoles();

            //convert the username to lowercase (not possible Matt und matt)
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            
            //checks if user already exists
            if (await UserExists(userForRegisterDto.Username))
            {
                throw new Exception("Username existiert bereits");
            }


            var userToCreate = new ApplicationUser
            {
                Username = userForRegisterDto.Username
            };
            var createdUser = await CreateUserToRegister(userToCreate, userForRegisterDto.Password);

            return createdUser;
        }
        private async Task<ApplicationUser> CreateUserToRegister(ApplicationUser user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            // Sets UserRole to "User"
            user = await SetUserRole(user, 1); 

            await _context.ApplicationUser.AddAsync(user);
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

        private async Task<ApplicationUser> SetUserRole(ApplicationUser user, int role)
        {
            if (await _context.Authorizations.AnyAsync(x => x.Id == role))
            {
                Authorization auth = await _context.Authorizations.FirstOrDefaultAsync(i => i.Id == role);
                user.Authorization = auth;   
            }
            return user;
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
        
        private async Task<ApplicationUser> GetUserForLogin(string username, string password)
        {
            //returns User with given username
            var user = await _context.ApplicationUser.Include(i => i.Authorization).FirstOrDefaultAsync(x => x.Username == username);
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
        

        private SecurityToken CreateToken(ApplicationUser user)
        {
            //The information in claim in the token can be accessed 
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim( ClaimTypes.Role, user.Authorization.Role),
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
            if (await _context.ApplicationUser.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }


        // checks if roles exist in db-table authorizations and creates them
        private void CheckAndCreateAuthRoles()
        {
            // Check if AuthorizationRole with id = 1 is user, else update/create role user with id 1
            Authorization auth = _context.Authorizations.FirstOrDefault(i => i.Id.Equals(1));
            if(auth != null && !auth.Role.Equals("user"))
            {
                auth.Role = "user";
                _context.Update(auth);
            }
            if(auth == null)
            {
                _context.Authorizations.Add(new Models.Authorization() { Role = "user", Id = 1 });
            }

            // check if roles administrator and council exist, else create them
            if (_context.Authorizations.FirstOrDefault(i => i.Role.Equals("council")) == null)
            {
                _context.Authorizations.Add(new Models.Authorization() { Role = "council"});
            }
            if (_context.Authorizations.FirstOrDefault(i => i.Role.Equals("administrator")) == null)
            {
                _context.Authorizations.Add(new Models.Authorization() { Role = "administrator"});
            }

            _context.SaveChanges();
        }

    }
}