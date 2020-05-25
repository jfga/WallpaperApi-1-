using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallpaperApi.Models
{
    public class WallhavenResponse
    {
        public int id { get; set; }
        public List<ImageInfo> results { get; set; }
    }
}
