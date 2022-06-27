using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        public HomeRepository(YourVitebskDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private bool HasPermission(int roleId, Helpers.RolePermission permission)
        {
            Models.RolePermission rolePermission = _context.RolePermissions.FirstOrDefault(x => x.Name == Enum.GetName(typeof(Helpers.RolePermission), permission));
            RolePermissionLink permissionLink = _context.RolePermissionLinks.FirstOrDefault(x => x.RoleId == roleId && x.RolePermissionId == rolePermission.RolePermissionId);
            if (permissionLink == null)
            {
                return false;
            }

            return true;
        }

        public HomeIndexViewModel Get()
        {
            User currentUser = _context.Users.First(x => x.Email == _httpContextAccessor.HttpContext.User.Identity.Name);
            Role role = _context.Roles.First(x => x.RoleId == currentUser.RoleId);
            return new HomeIndexViewModel()
            {
                FirstName = currentUser.FirstName,
                RoleName = role.Name,
                TotalUsers = _context.Users.Count(),
                TotalCafes = _context.Cafes.Count(),
                TotalPosters = _context.Posters.Count(),
                TotalVacancies = _context.Vacancies.Count(),
                TotalComments = _context.Comments.Count(),
                UsersGet = HasPermission((int)role.RoleId, Helpers.RolePermission.UsersGet),
                UsersCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.UsersCreate),
                UsersUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.UsersUpdate),
                UsersDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.UsersDelete),
                RolesGet = HasPermission((int)role.RoleId, Helpers.RolePermission.RolesGet),
                RolesCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.RolesCreate),
                RolesUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.RolesUpdate),
                RolesDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.RolesDelete),
                NewsGet = HasPermission((int)role.RoleId, Helpers.RolePermission.NewsGet),
                NewsCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.NewsCreate),
                NewsUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.NewsUpdate),
                NewsDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.NewsDelete),
                CafesGet = HasPermission((int)role.RoleId, Helpers.RolePermission.CafesGet),
                CafesCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.CafesCreate),
                CafesUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.CafesUpdate),
                CafesDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.CafesDelete),
                PostersGet = HasPermission((int)role.RoleId, Helpers.RolePermission.PostersGet),
                PostersCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.PostersCreate),
                PostersUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.PostersUpdate),
                PostersDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.PostersDelete),
                VacanciesGet = HasPermission((int)role.RoleId, Helpers.RolePermission.VacanciesGet),
                VacanciesCreate = HasPermission((int)role.RoleId, Helpers.RolePermission.VacanciesCreate),
                VacanciesUpdate = HasPermission((int)role.RoleId, Helpers.RolePermission.VacanciesUpdate),
                VacanciesDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.VacanciesDelete),
                CommentsGet = HasPermission((int)role.RoleId, Helpers.RolePermission.CommentsGet),
                CommentsDelete = HasPermission((int)role.RoleId, Helpers.RolePermission.CommentsDelete),
            };

        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
