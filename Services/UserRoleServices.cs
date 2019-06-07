using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UserRolesManagement.DataModels;
using UserRolesManagement.Models;

namespace UserRolesManagement.Services
{
    public class UserRoleServices
    {
        private UserRoleManagementEntities db = new UserRoleManagementEntities();

        public List<UserRoleDataTable> GetAllUserRoles()
        {
            //           List<UserRoles> data = (from ur in db.UserRoles
            //                                   join
            //           u in db.Users on ur.UserID equals u.Id
            //                                   join
            //r in db.Roles on ur.RoleID equals r.Id
            //                                   select (x => new UserRoles()
            //                                   {
            //                                       UserName = u.Name,
            //                                       RoleName = r.Name,
            //                                       UserID = u.Id,
            //                                       RoleID = r.Id,
            //                                       Id = x.id
            //                                   });

            var data = db.UserRoles.Select(x => new UserRoleDataTable()
            {
                UserName = x.Role.Name,
                RoleName = x.User.Name
            }).ToList();
            //automapper


            return data;
        }
        public UserRoleDataTable GetUser(int id)
        {
            var userRole = db.UserRoles.Where(x => x.Id == id).Select(x => new UserRoleDataTable()
            {
                UserName = x.Role.Name,
                RoleName = x.User.Name
            }).FirstOrDefault();
            if (userRole == null)
            {
                return null;
            }
            return userRole;
        }
        public void UpdateUserById(int id, UserRoleDataTable userRole)
        {
            var dbUserRole = db.UserRoles.Where(x => x.Id == id).FirstOrDefault();
            if (dbUserRole != null)
            {
                dbUserRole.User.Name = userRole.UserName;
                dbUserRole.Role.Name = userRole.RoleName;
                db.Entry(dbUserRole).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }

        public void AddUserRole(UserRoles userRole)
        {
            try
            {
                db.UserRoles.Add(new Models.UserRole { UserID=userRole.UserID,RoleID=userRole.RoleID});
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public UserRoles DeleteUserRole(int id)
        {
            var userRole = db.UserRoles.Where(x => x.UserID == id).FirstOrDefault();
            if (userRole != null)
            {
                db.UserRoles.Remove(userRole);
            }
            var user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return new DataModels.User { Name = user.Name, Email = user.EmailId };


        }
        internal bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
    }
}