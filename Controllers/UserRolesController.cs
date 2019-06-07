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
        public UserRoleServices UserRoleServices { get; set; }

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
        public IHttpActionResult UpdateUserRole(int id, UserRoles userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedUserRole = UserRoleServices.UpdateUserById(id, userRole);

            try
            {
                if (updatedUserRole == null)
                {
                    return NotFound();
                }

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

            return Ok(updatedUserRole);
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
            bool res = UserRoleServices.DeleteUserRole(id);
            if (res != true)
                return NotFound();
            else
                return Ok(userRoles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserRoleServices.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRoleExists(int id)
        {
            return UserRoleServices.UserRoleExists(id);
        }
    }
}