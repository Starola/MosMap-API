using System.Threading.Tasks;
 using MosMap_API.Models;
 
 namespace MosMap_API.ServiceInterfaces
 {
     public interface IUserService
     {
         /*
          * If the current HttpRequest has a correct token: true 
          */
         bool IsAuthenticated();
         int GetUserId();

         string GetUserName();

         bool IsAdmin();

         bool IsCouncil();
     }

 }