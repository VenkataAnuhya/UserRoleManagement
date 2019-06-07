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
using UserRolesManagement.Services;

namespace UserRolesManagement.Controllers
{
    public class UsersController : ApiController
    {
        public UserServices UserServices { get; set; }
        public UsersController()
        {
            UserServices = new UserServices();
        }

        [HttpGet]
        [Route("Users")]
        [ResponseType(typeof(List<User>))]
        public IHttpActionResult GetUsers()
        {
            List<User> data = UserServices.GetAllUsers();
            return Ok(data);
            
        }

        [HttpGet]
        [Route("GetUserById")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUserById(int id)
        {
            User user = UserServices.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserServices.UpdateUserById(id, user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpPost]
        [Route("AddUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserServices.AddUser(user);
            return Ok(user);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = UserServices.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserServices.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return UserServices.UserExists(id);
        }
    }
}