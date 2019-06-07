using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UserRolesManagement.DataModels;
using UserRolesManagement.Models;
using User = UserRolesManagement.DataModels.User;

namespace UserRolesManagement.Services
{
    public class UserServices : IDisposable
    {
        private UserRoleManagementEntities db = new UserRoleManagementEntities();
        public List<User> GetAllUsers()
        {
            List<User> data =  db.Users.Select(x => new User() {
                Id = x.Id,
                Name = x.Name,
                Email= x.EmailId
            }).ToList();
            return data;
        }
        public User GetUser(int id)
        {
           User user = db.Users.Where(x=>x.Id==id).Select(x=> new User()
           {
               Id=x.Id,
               Name = x.Name,
               Email = x.EmailId
           }).FirstOrDefault();
            if (user == null)
            {
               return null;
            }
            return user;
        }

        public void UpdateUserById(int id, User user)
        {
            var dbUser = db.Users.Where(x => x.Id == id).FirstOrDefault();
            if(dbUser != null)
            {
                dbUser.Name = user.Name;

                db.Entry(dbUser).State = EntityState.Modified;
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

        public void AddUser(User user)
        {
            try
            {
                db.Users.Add(new Models.User { Name = user.Name, EmailId = user.Email });
              db.SaveChanges();
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public User DeleteUser(int id)
        {
            var userRole = db.UserRoles.Where(x => x.UserID == id).FirstOrDefault();
            db.UserRoles.Remove(userRole);
            var user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return new DataModels.User { Name = user.Name, Email =user.EmailId};


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