using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MosMap_API.Data;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DataContext _context;

        public UserService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        
        public bool IsAdmin()
        {
            return CheckRole("administrator");
        }

        public bool IsCouncil()
        {
            return CheckRole("council");
        }

        private bool CheckRole(string role)
        {
            if(!IsAuthenticated()) throw new Exception("Kein User angemeldet!");
            var userId = GetUserId();
            var context = _context.ApplicationUser.Include(i => i.Authorization).FirstOrDefault( x => x.Id == userId);
            if (context == null) throw new Exception("User kann nicht gefunden werden");
            var authorization = context.Authorization;
            if (authorization.Role == role) return true; 
            return false;
        }


        public int GetUserId()
        {
            var claim = UserClaims();
            var userId = claim[0].Value;
            return int.Parse(userId);
        }

        public string GetUserName()
        {
            var claim = UserClaims();
            var userName = claim[1].Value;
            return userName;
        }

        /*
         * Gives a List of all claims from the token back
         */
        private  IList<Claim> UserClaims()
        {
            if(!IsAuthenticated()) throw new Exception("Kein User angemeldet!");
            var context = httpContextAccessor.HttpContext;
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IList<Claim> claim = identity.Claims.ToList();
                return claim;
            }
            throw new Exception("Unerwarteter Fehler bei UserIdentity");
        }
        /*
        * If the current HttpRequest has a correct token: true 
        */
        public bool IsAuthenticated()
        {
            var context = httpContextAccessor.HttpContext;
            if (context.User.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }
    }
}