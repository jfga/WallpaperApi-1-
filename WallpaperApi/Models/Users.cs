using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallpaperApi.Models
{
    public class Users
    {
       public int Id { get;  set; }
       public List<PhotoInfo> Images = new List<PhotoInfo>();

    }
}
