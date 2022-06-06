using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class UsersService : IUsersService
    {
        private readonly YourVitebskDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public UsersService(YourVitebskDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IEnumerable<APIModels.UsersListItem>> GetAllUsers(int id)
        {
            IEnumerable<APIModels.UsersListItem> result = new List<APIModels.UsersListItem>();
            IEnumerable<Models.User> users = await _context.Users.Include(x => x.UserDatum).Where(x => x.IsVisible == true && x.UserId != id).ToListAsync();
            foreach (var user in users)
            {
                string image = "";
                if (Directory.Exists($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}"))
                {
                    image = Directory.GetFiles($"{_appEnvironment.WebRootPath}/images/users/{user.UserId}").Select(x => Path.GetFileName(x)).First();
                }

                result = result.Append(new APIModels.UsersListItem()
                {
                    UserId = (int)user.UserId,
                    FirstName = user.UserDatum.FirstName,
                    LastName = user.UserDatum.LastName,
                    PhoneNumber = user.UserDatum.PhoneNumber,
                    Image = image
                });
            }

            return result;
        }
    }
}
