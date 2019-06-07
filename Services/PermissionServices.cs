using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UserRolesManagement.Models;
using Permission = UserRolesManagement.DataModels.Permission;

namespace UserRolesManagement.Services
{
    public class PermissionServices
    {
        private UserRoleManagementEntities db = new UserRoleManagementEntities();
        public List<Permission> GetAllPermissions()
        {
            List<Permission> data = db.Permissions.Select(x => new Permission()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return data;
        }

        public Permission GetPermission(int id)
        {
            Permission permission = db.Permissions.Where(x => x.Id == id).Select(x => new Permission()
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefault();
            if (permission == null)
            {
                return null;
            }
            return permission;
        }
        public void UpdatePermissionById(int id, Permission permission)
        {
            var dbpermission = db.Permissions.Where(x => x.Id == id).FirstOrDefault();
            if (dbpermission != null)
            {
                dbpermission.Name = permission.Name;
                db.Entry(dbpermission).State = EntityState.Modified;
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

        public void AddPermission(Permission permission)
        {
            db.Permissions.Add(new Models.Permission { Name = permission.Name });
            db.SaveChanges();
        }

        public Permission DeletePermission(int id)
        {
            var permissionRole = db.PermissionRoles.Where(x => x.RoleID == id).FirstOrDefault();
            if (permissionRole != null)
            {
                db.PermissionRoles.Remove(permissionRole);
            }
            var permission = db.Permissions.Where(x => x.Id == id).FirstOrDefault();
            if (permission != null)
            {
                db.Permissions.Remove(permission);
            }
            else
                return null;
            db.SaveChanges();
            return new DataModels.Permission { Name = permission.Name };
        }

        public bool PermissionExists(int id)
        {
            return db.Permissions.Count(e => e.Id == id) > 0;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}

    
