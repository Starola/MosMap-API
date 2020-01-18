using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class LocationForAcceptDto
    {
        public int LocationId { get; set; }
        public bool Accepted { get; set; }
    }
}
