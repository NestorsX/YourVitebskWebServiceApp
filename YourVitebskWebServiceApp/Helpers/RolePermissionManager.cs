using System.Linq;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Helpers
{
    public class RolePermissionManager
    {
        private readonly YourVitebskDBContext _context;
        public RolePermissionManager(YourVitebskDBContext context)
        {
            _context = context;
        }

        public bool HasPermission(string userEmail, string requiredPermission)
        {
            User user = _context.Users.First(x => x.Email == userEmail);
            Models.RolePermission rolePermission = _context.RolePermissions.First(x => x.Name == requiredPermission);
            if (_context.RolePermissionLinks.Any(x => x.RoleId == user.RoleId && x.RolePermissionId == rolePermission.RolePermissionId))
            {
                return true;
            }

            return false;
        }
    }
}
