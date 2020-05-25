using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallpaperApi.Models
{
    public class Commands
    {

        public IMongoCollection<Users> userInfo;
        public Commands()
        {
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0-s3p8h.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("ImageDataBase");
            userInfo = database.GetCollection<Users>("Favourite");
        }
        public Users Get(string id) =>
            userInfo.Find<Users>(user => user.Id == Convert.ToInt64(id)).FirstOrDefault();

        public async Task<Users> Create(Users user)
        {
            await userInfo.InsertOneAsync(user);
            return user;
        }

        public async void Update(string id, Users userIn) =>
            await userInfo.ReplaceOneAsync(user => user.Id == Convert.ToInt64(id), userIn);



        public string WallpaperFindColor(FindUser item)
        {
            string link = $"https://api.unsplash.com/search/photos?query=/color={item.color}&client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8";
            return (link);
        }

        public string WallpaperRandom(FindUser item)
        {
            string link = $"https://api.unsplash.com/photos/random/?client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8";
            return (link);
        }

        public string FindByCategory(FindUser item)
        {
            string link = $"https://api.unsplash.com/search/photos?query={item.category}&client_id=JGYSsjXO8ANdFCsiBrNZVmi3yXfOcSM5VD0jU8EpeY8";
            return (link);
        }



        
    }
}
