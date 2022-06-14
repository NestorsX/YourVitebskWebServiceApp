using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Models
{
    public class UserFilterer : IFilterer
    {
        public UserFilterer(IEnumerable<UserViewModel> list, int? role, string searchLine)
        {
            if (role != null)
            {
                list = list.Where(x => x.RoleId == role);
            }

            if (!string.IsNullOrWhiteSpace(searchLine))
            {
                list = list.Where(x => x.Email.Contains(searchLine) ||
                                         x.FirstName.Contains(searchLine) ||
                                         x.LastName.Contains(searchLine) ||
                                         x.PhoneNumber.Contains(searchLine)
                );
            }
        }

        public IEnumerable<UserViewModel> List { get; private set; }
        public int? RoleId { get; private set; }
        public string SearchLine { get; private set; }
    }
}
