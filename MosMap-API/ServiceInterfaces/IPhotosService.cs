using System.Threading.Tasks;
using MosMap_API.Dtos;
using MosMap_API.Models;

namespace MosMap_API.ServiceInterfaces
{
    public interface IPhotosService
    {
        Task<PhotoForReturnDto> AddPhotoForLocation(int locationId, PhotoForCreationDto photoForCreationDto);
        Task<PhotoForReturnDto> GetPhoto(int id);
    }
}