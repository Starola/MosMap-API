using MosMap_API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface IGeoJsonService
    {
        /// <summary>
        /// Converts LocationDto to LocationAsGeoJsonDto
        /// </summary>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        Task<LocationAsGeoJsonDto> ConvertLocationDtoToGeoJson(LocationDto locationDto);

        /// <summary>
        /// Converts List of LocationDtos to List of LocationAsGeoJsonDtos
        /// </summary>
        /// <param name="locationDtos"></param>
        /// <returns></returns>
        Task<List<LocationAsGeoJsonDto>> ConvertLocationDtoToGeoJson(IEnumerable<LocationByCategoryDto> locationDtos);

    }
}
