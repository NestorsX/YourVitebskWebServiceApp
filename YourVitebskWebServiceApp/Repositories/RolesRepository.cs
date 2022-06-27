using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.ViewModels;

namespace YourVitebskWebServiceApp.Repositories
{
    public class RolesRepository : IRoleRepository
    {
        private readonly YourVitebskDBContext _context;
        private readonly Helpers.RolePermissionManager _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;
        private readonly List<RolePermissionLink> _permissionLinks;
        private readonly List<RolePermission> _permissions;

        public RolesRepository(YourVitebskDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _roleManager = new Helpers.RolePermissionManager(_context);
            _httpContextAccessor = httpContextAccessor;
            _permissionLinks = _context.RolePermissionLinks.ToList();
            _permissions = _context.RolePermissions.ToList();
        }

        private bool HasPermission(int roleId, Helpers.RolePermission permission)
        {
            RolePermission rolePermission = _permissions.FirstOrDefault(x => x.Name == Enum.GetName(typeof(Helpers.RolePermission), permission));
            RolePermissionLink permissionLink = _context.RolePermissionLinks.FirstOrDefault(x => x.RoleId == roleId && x.RolePermissionId == rolePermission.RolePermissionId);
            if (permissionLink == null)
            {
                return false;
            }

            return true;
        }

        private void AddPermission(int roleId, Helpers.RolePermission permission)
        {
            RolePermission rolePermission = _permissions.FirstOrDefault(x => x.Name == Enum.GetName(typeof(Helpers.RolePermission), permission));
            RolePermissionLink permissionLink = _context.RolePermissionLinks.FirstOrDefault(x => x.RoleId == roleId && x.RolePermissionId == rolePermission.RolePermissionId);
            if (permissionLink != null)
            {
                return;
            }

            _context.RolePermissionLinks.Add(new RolePermissionLink
            {
                RolePermissionLinkId = null,
                RoleId = roleId,
                RolePermissionId = (int)_permissions.First(x => x.Name == Enum.GetName(typeof(Helpers.RolePermission), permission)).RolePermissionId
            });

            _context.SaveChanges();
        }

        private void UpdatePermission(int roleId, Helpers.RolePermission permission, bool hasPermission)
        {
            if (hasPermission)
            {
                AddPermission(roleId, permission);
                return;
            }

            DeletePermission(roleId, permission);
        }

        private void DeletePermission(int roleId, Helpers.RolePermission permission)
        {
            RolePermission rolePermission = _permissions.FirstOrDefault(x => x.Name == Enum.GetName(typeof(Helpers.RolePermission), permission));
            RolePermissionLink permissionLink = _context.RolePermissionLinks.FirstOrDefault(x => x.RoleId == roleId && x.RolePermissionId == rolePermission.RolePermissionId);
            if (permissionLink == null)
            {
                return;
            }

            _context.Remove(permissionLink);
            _context.SaveChanges();
        }

        public bool CheckRolePermission(string permission)
        {
            return _roleManager.HasPermission(_httpContextAccessor.HttpContext.User.Identity.Name, permission);
        }

        public IEnumerable<RoleViewModel> Get()
        {
            var result = new List<RoleViewModel>();
            IEnumerable<Role> roles = _context.Roles.ToList();
            foreach (var role in roles)
            {
                result.Add(GetForView((int)role.RoleId));
            }

            return result;
        }

        public RoleViewModel GetForView(int id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (role == null)
            {
                return null;
            }

            var permissionList = new List<string>();
            foreach (var permission in _permissionLinks.Where(x => x.RoleId == id))
            {
                permissionList.Add(_permissions.First(x => x.RolePermissionId == permission.RolePermissionId).Name);
            }

            var result = new RoleViewModel()
            {
                RoleId = role.RoleId,
                Name = role.Name,
                Permissions = permissionList
            };

            return result;
        }

        public RoleDTOViewModel GetForEdit(int id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (role == null)
            {
                return null;
            }

            var result = new RoleDTOViewModel()
            {
                RoleId = role.RoleId,
                Name = role.Name,
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

            return result;
        }

        public void Create(RoleDTOViewModel role)
        {
            var newRole = new Role()
            {
                RoleId = null,
                Name = role.Name
            };

            _context.Roles.Add(newRole);
            _context.SaveChanges();
            #region Users
            // Users ------------------------------------------------------------------------- //
            if (role.UsersGet || role.UsersCreate || role.UsersUpdate || role.UsersDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.UsersGet);
            }

            if (role.UsersCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.UsersCreate);
            }

            if (role.UsersUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.UsersUpdate);
            }

            if (role.UsersDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.UsersDelete);
            }
            #endregion
            #region Roles
            //Roles ------------------------------------------------------------------------- //
            if (role.RolesGet || role.RolesCreate || role.RolesUpdate || role.RolesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.RolesGet);
            }

            if (role.RolesCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.RolesCreate);
            }

            if (role.RolesUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.RolesUpdate);
            }

            if (role.RolesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.RolesDelete);
            }
            #endregion
            #region News
            //News ------------------------------------------------------------------------- //
            if (role.NewsGet || role.NewsCreate || role.NewsUpdate || role.NewsDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.NewsGet);
            }

            if (role.NewsCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.NewsCreate);
            }

            if (role.NewsUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.NewsUpdate);
            }

            if (role.NewsDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.NewsDelete);
            }
            #endregion News
            #region Cafes
            //Cafes ------------------------------------------------------------------------- //
            if (role.CafesGet || role.CafesCreate || role.CafesUpdate || role.CafesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CafesGet);
            }

            if (role.CafesCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CafesCreate);
            }

            if (role.CafesUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CafesUpdate);
            }

            if (role.CafesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CafesDelete);
            }
            #endregion
            #region Posters
            //Posters ------------------------------------------------------------------------- //
            if (role.PostersGet || role.PostersCreate || role.PostersUpdate || role.PostersDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.PostersGet);
            }

            if (role.PostersCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.PostersCreate);
            }

            if (role.PostersUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.PostersUpdate);
            }

            if (role.PostersDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.PostersDelete);
            }
            #endregion
            #region Vacancies
            //Posters ------------------------------------------------------------------------- //
            if (role.VacanciesGet || role.VacanciesCreate || role.VacanciesUpdate || role.VacanciesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.VacanciesGet);
            }

            if (role.VacanciesCreate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.VacanciesCreate);
            }

            if (role.VacanciesUpdate)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.VacanciesUpdate);
            }

            if (role.VacanciesDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.VacanciesDelete);
            }
            #endregion
            #region Comments
            //Comments ------------------------------------------------------------------------- //
            if (role.CommentsGet || role.CommentsDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CommentsGet);
            }

            if (role.CommentsDelete)
            {
                AddPermission((int)newRole.RoleId, Helpers.RolePermission.CommentsDelete);
            }
            #endregion
        }

        public void Update(RoleDTOViewModel role)
        {
            var previousRoleDTOViewModel = GetForEdit((int)role.RoleId);
            var currentRole = _context.Roles.FirstOrDefault(x => x.RoleId == role.RoleId);
            currentRole.Name = role.Name;
            _context.Roles.Update(currentRole);
            _context.SaveChanges();
            #region Users
            // Users ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.UsersGet != role.UsersGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.UsersGet, role.UsersGet);
                if (!role.UsersGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.UsersCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.UsersUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.UsersDelete);
                    role.UsersCreate = false;
                    role.UsersUpdate = false;
                    role.UsersDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.UsersCreate, role.UsersCreate);
            if (role.UsersCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.UsersGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.UsersUpdate, role.UsersUpdate);
            if (role.UsersUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.UsersGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.UsersDelete, role.UsersDelete);
            if (role.UsersDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.UsersGet);
            }
            #endregion
            #region Roles
            //Roles ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.RolesGet != role.RolesGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.RolesGet, role.RolesGet);
                if (!role.RolesGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.RolesCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.RolesUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.RolesDelete);
                    role.RolesCreate = false;
                    role.RolesUpdate = false;
                    role.RolesDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.RolesCreate, role.RolesCreate);
            if (role.RolesCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.RolesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.RolesUpdate, role.RolesUpdate);
            if (role.RolesUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.RolesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.RolesDelete, role.RolesDelete);
            if (role.RolesDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.RolesGet);
            }
            #endregion
            #region News
            //News ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.NewsGet != role.NewsGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.NewsGet, role.NewsGet);
                if (!role.NewsGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.NewsCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.NewsUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.NewsDelete);
                    role.NewsCreate = false;
                    role.NewsUpdate = false;
                    role.NewsDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.NewsCreate, role.NewsCreate);
            if (role.NewsCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.NewsGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.NewsUpdate, role.NewsUpdate);
            if (role.NewsUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.NewsGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.NewsDelete, role.NewsDelete);
            if (role.NewsDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.NewsGet);
            }
            #endregion News
            #region Cafes
            //Cafes ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.CafesGet != role.CafesGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.CafesGet, role.CafesGet);
                if (!role.CafesGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.CafesCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.CafesUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.CafesDelete);
                    role.CafesCreate = false;
                    role.CafesUpdate = false;
                    role.CafesDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.CafesCreate, role.CafesCreate);
            if (role.CafesCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.CafesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.CafesUpdate, role.CafesUpdate);
            if (role.CafesUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.CafesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.CafesDelete, role.CafesDelete);
            if (role.CafesDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.CafesGet);
            }
            #endregion
            #region Posters
            //Posters ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.PostersGet != role.PostersGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.PostersGet, role.PostersGet);
                if (!role.PostersGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.PostersCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.PostersUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.PostersDelete);
                    role.PostersCreate = false;
                    role.PostersUpdate = false;
                    role.PostersDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.PostersCreate, role.PostersCreate);
            if (role.PostersCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.PostersGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.PostersUpdate, role.PostersUpdate);
            if (role.PostersUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.PostersGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.PostersDelete, role.PostersDelete);
            if (role.PostersDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.PostersGet);
            }
            #endregion
            #region Vacancies
            //Posters ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.VacanciesGet != role.VacanciesGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.VacanciesGet, role.VacanciesGet);
                if (!role.VacanciesGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.VacanciesCreate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.VacanciesUpdate);
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.VacanciesDelete);
                    role.VacanciesCreate = false;
                    role.VacanciesUpdate = false;
                    role.VacanciesDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.VacanciesCreate, role.VacanciesCreate);
            if (role.VacanciesCreate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.VacanciesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.VacanciesUpdate, role.VacanciesUpdate);
            if (role.VacanciesUpdate)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.VacanciesGet);
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.VacanciesDelete, role.VacanciesDelete);
            if (role.VacanciesDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.VacanciesGet);
            }
            #endregion
            #region Comments
            //Comments ------------------------------------------------------------------------- //
            if (previousRoleDTOViewModel.CommentsGet != role.CommentsGet)
            {
                UpdatePermission((int)currentRole.RoleId, Helpers.RolePermission.CommentsGet, role.CommentsGet);
                if (!role.CommentsGet)
                {
                    DeletePermission((int)role.RoleId, Helpers.RolePermission.CommentsDelete);
                    role.CommentsDelete = false;
                }
            }

            UpdatePermission((int)role.RoleId, Helpers.RolePermission.CommentsDelete, role.CommentsDelete);
            if (role.CommentsDelete)
            {
                AddPermission((int)role.RoleId, Helpers.RolePermission.CommentsGet);
            }
            #endregion

        }

        public void Delete(int id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (role == null)
            {
                return;
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();
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