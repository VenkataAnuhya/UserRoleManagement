using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UserRolesManagement.DataModels;
using UserRolesManagement.Models;
using Role = UserRolesManagement.DataModels.Role;

namespace UserRolesManagement.Services
{
    public class RoleServices : IDisposable
    {
        private UserRoleManagementEntities db = new UserRoleManagementEntities();

        // GET: api/Roles
        public List<Role> GetAllRoles()
        {
            List<Role> data = db.Roles.Select(x => new Role()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return data;
        }

        public Role GetRole(int id)
        {
            Role role = db.Roles.Where(x => x.Id == id).Select(x => new Role()
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefault();
            if (role == null)
            {
                return null;
            }
            return role;
        }

        public void UpdateRoleById(int id, Role role)
        {
            var dbRole = db.Roles.Where(x => x.Id == id).FirstOrDefault();
            if (dbRole != null)
            {
                dbRole.Name = role.Name;
                db.Entry(dbRole).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }

                return;
            }
        }

        public void AddRole(Role role)
        {
            db.Roles.Add(new Models.Role { Name = role.Name });
            db.SaveChanges();
        }

        public Role DeleteRole(int id)
        {
            var permissionRole = db.PermissionRoles.Where(x => x.RoleID == id).FirstOrDefault();
            if (permissionRole != null)
            {
             db.PermissionRoles.Remove(permissionRole);
            }
            var userRole = db.UserRoles.Where(x => x.RoleID == id).FirstOrDefault();
            if (userRole != null)
            {
                db.UserRoles.Remove(userRole);
            }
            var role = db.Roles.Where(x => x.Id == id).FirstOrDefault();
            if (role != null)
            {
                db.Roles.Remove(role);
            }
            else
                return null;
            db.SaveChanges();
            return new DataModels.Role { Name = role.Name};
        }

        public bool RoleExists(int id)
        {
            return db.Roles.Count(e => e.Id == id) > 0;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}