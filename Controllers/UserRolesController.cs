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
using UserRolesManagement.Services;

namespace UserRolesManagement.Controllers
{
    public class UserRolesController : ApiController
    {
        public UserRoleServices  UserRoleServices { get; set; }

        public UserRoles userRoles;
        public UserRolesController()
        {
            UserRoleServices = new UserRoleServices();
        }

        [HttpGet]
        [Route("UserRoles")]
        public IHttpActionResult GetUserRoles()
        {
            List<UserRoleDataTable> data = UserRoleServices.GetAllUserRoles();
            return Ok(data);
        }

        [ResponseType(typeof(UserRole))]
        [HttpGet]
        [Route("GetUserRole")]
        public IHttpActionResult GetUserRole(int id)
        {
            var userRole = UserRoleServices.GetUser(id);
            if (userRole == null)
            {
                return NotFound();
            }

            return Ok(userRole);
        }

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("UpdateUserRole")]
        public IHttpActionResult UpdateUserRole(int id, UserRoleDataTable userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id !=userRoles.Id )
            {
                return BadRequest();
            }

            try
            {
                UserRoleServices.UpdateUserById(id, userRole);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(UserRole))]
        [HttpPost]
        [Route("AddUserRole")]
        public IHttpActionResult AddUserRole(UserRoles userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRoleServices.AddUserRole(userRole);
            return Ok(userRole);
        }

        [ResponseType(typeof(UserRole))]
        [HttpDelete]
        [Route("DeleteUserRole")]
        public IHttpActionResult DeleteUserRole(int id)
        {
            
            if (userRole == null)
            {
                return NotFound();
            }

            db.UserRoles.Remove(userRole);
            db.SaveChanges();

            return Ok(userRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRoleExists(int id)
        {
            return db.UserRoles.Count(e => e.Id == id) > 0;
        }
    }
}