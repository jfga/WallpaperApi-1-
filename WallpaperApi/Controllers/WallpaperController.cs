using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WallpaperApi.Models;
using Newtonsoft.Json;

namespace WallpaperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WallpaperController : ControllerBase
    {
        Commands commands = new Commands();

        [HttpPost]
        [Route("Color")]
        public async Task<ObjectResult> FindByColor([FromBody] FindUser item)
        {
            string link = commands.WallpaperFindColor(item);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(link); //item1 - ссылка

            WallhavenResponse wallhavenResponse = JsonConvert.DeserializeObject<WallhavenResponse>(content);
            
            return new ObjectResult(wallhavenResponse);
        }
        [HttpPost]
        [Route("Category")]
        public async Task<ObjectResult> FindCategory([FromBody] FindUser item)
        {
            string link = commands.FindByCategory(item);
            using var client = new HttpClient();

            var content = await client.GetStringAsync(link);
            WallhavenResponse wallhavenResponse = JsonConvert.DeserializeObject<WallhavenResponse>(content);
            wallhavenResponse.id = item.id;
            return new ObjectResult(wallhavenResponse);
        }
        [HttpGet]
        [Route("random")]
        public async Task<ObjectResult> Random()
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://api.unsplash.com/photos/random/?client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8");
            RandomResponse random = JsonConvert.DeserializeObject<RandomResponse>(content);
            return new ObjectResult(random);
        }
        [HttpPost]
        [Route("AddFavourite")]
        public async Task<ObjectResult> AddFavorite([FromBody] FindUser item)
        {
          

            Users users_1 = commands.Get(Convert.ToString(item.id));

            DataBase dataBase = new DataBase();
            dataBase.users.Add(users_1);


            if (users_1 == null)
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"https://api.unsplash.com/photos/{item.photoId}/?client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8");
                RandomResponse wallhaven = JsonConvert.DeserializeObject<RandomResponse>(content);

                Users users_2 = new Users();
                PhotoInfo photoInfo = new PhotoInfo();
                photoInfo.Id = item.photoId;
                photoInfo.Link = wallhaven.urls.full;

                users_2.Id = item.id;
                users_2.Images.Add(photoInfo);
                await commands.Create(users_2);

                return new ObjectResult("Added");
            }
            else
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"https://api.unsplash.com/photos/{item.photoId}/?client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8");
                RandomResponse wallhaven = JsonConvert.DeserializeObject<RandomResponse>(content);

               
                bool imagecontains = false;
                for (int i = 0; i < users_1.Images.Count; i++)
                {
                    if(users_1.Images[i].Id == item.photoId)
                    {
                        imagecontains = true;
                        break;
                    }

                }

                if ( imagecontains == false)
                {
                    PhotoInfo photoInfo = new PhotoInfo();
                    photoInfo.Id = item.photoId;
                    photoInfo.Link = wallhaven.urls.full;

                    users_1.Id = item.id;
                    users_1.Images.Add(photoInfo);
                    commands.Update(Convert.ToString(item.id) ,users_1);
                    return new ObjectResult("Added");
                }
                else
                {
                    return new ObjectResult("You alrady add this image ");
                }
            }

            
        }
        [HttpPut]   /////Put
        [Route("DelFavourite")]

        public async Task<ObjectResult> DelFavourite([FromBody] FindUser item)
        {
            
            Users users_1 = commands.Get(Convert.ToString(item.id));

            DataBase dataBase = new DataBase();
            dataBase.users.Add(users_1);


            if (users_1 == null)
            {
              
                return new ObjectResult("BAD");
            }
            else
            {
                using var client = new HttpClient();
                var content = await client.GetStringAsync($"https://api.unsplash.com/photos/{item.photoId}/?client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8");
                RandomResponse wallhaven = JsonConvert.DeserializeObject<RandomResponse>(content);


                bool imagecontains = false;
                int index = 0;
                for (int i = 0; i < users_1.Images.Count; i++)
                {
                    if (users_1.Images[i].Id == item.photoId)
                    {
                        index = i;
                        imagecontains = true;
                        break;
                    }

                }

                if (imagecontains == false)
                {
                    
                    return new ObjectResult("BAD");
                }
                else
                {
                    dataBase.users[0].Images.RemoveAt(index);
                    commands.Update(Convert.ToString(item.id), dataBase.users[0]);
                    return new ObjectResult("Deleted");
                }
            }
        }

        
        [HttpPost]
        [Route("GetFavourite")]
        public async Task<ObjectResult> GetFavorite([FromBody] FindUser item)
        {
            Users users = commands.Get(Convert.ToString(item.id));

            DataBase dataBase = new DataBase();
            dataBase.users.Add(users);

            if (users == null)
            {
                return new ObjectResult("BAD");
            }


            else  
            {
                return new ObjectResult(dataBase.users[0].Images);
            }
           
        }
    }
}