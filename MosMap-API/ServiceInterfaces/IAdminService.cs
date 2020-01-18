using MosMap_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<LocationForAdminDto>> GetAllUncheckedLocations();
        void AcceptLocation(LocationForAcceptDto locationForAcceptDto); 
    }
}
