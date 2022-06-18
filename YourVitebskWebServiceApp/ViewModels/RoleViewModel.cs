using System.Collections.Generic;

namespace YourVitebskWebServiceApp.ViewModels
{
    public class RoleViewModel
    {
        public int? RoleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
