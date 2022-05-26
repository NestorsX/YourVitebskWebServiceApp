using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;

namespace YourVitebskWebServiceApp.APIServices
{
    public class UsersService : IUsersService
    {
        private readonly YourVitebskDBContext _context;

        public UsersService(YourVitebskDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIModels.UsersListItem>> GetAllUsers(int id)
        {
            IEnumerable<APIModels.UsersListItem> result = new List<APIModels.UsersListItem>();
            IEnumerable<Models.User> users = await _context.Users.Include(x => x.UserDatum).Where(x => x.IsVisible == true && x.UserId != id).ToListAsync();
            foreach (var user in users)
            {
                result = result.Append(new APIModels.UsersListItem()
                {
                    UserId = (int)user.UserId,
                    FirstName = user.UserDatum.FirstName,
                    LastName = user.UserDatum.LastName,
                    PhoneNumber = user.UserDatum.PhoneNumber
                });
            }

            return result;
        }
    }
}
