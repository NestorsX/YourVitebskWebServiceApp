using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Helpers.Filterers
{
    public class UserFilterer
    {
        public UserFilterer(List<RoleViewModel> roles, int? role, string searchLine)
        {
            roles.Insert(0, new RoleViewModel { RoleId = 0, Name = "Все роли" });
            Roles = new SelectList(roles, "RoleId", "Name", role);
            SelectedRole = role;
            SearchLine = searchLine;
        }

        public SelectList Roles { get; private set; }
        public int? SelectedRole { get; private set; }
        public string SearchLine { get; private set; }
    }
}
