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
using Role = UserRolesManagement.DataModels.Role;

namespace UserRolesManagement.Controllers
{
    public class RolesController : ApiController
    {
        public RoleServices RoleServices { get; set; }
        public RolesController()
        {
            RoleServices = new RoleServices();
        }

        [Route("Roles")]
        [HttpGet]
        public IHttpActionResult GetRoles()
        {
            List<Role> data = RoleServices.GetAllRoles();
            return Ok(data);
        }

        [Route("GetRoleById")]
        [HttpGet]
        [ResponseType(typeof(Models.Role))]
        public IHttpActionResult GetRoleById(int id)
        {
            Role role = RoleServices.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("UpdateRole")]
        public IHttpActionResult UpdateRole(int id, Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                RoleServices.UpdateRoleById(id, role);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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
        [Route("AddRole")]
        [ResponseType(typeof(Models.Role))]
        public IHttpActionResult AddRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            RoleServices.AddRole(role);
            return Ok(role);
        }

        [HttpDelete]
        [Route("DeleteRole")]
        [ResponseType(typeof(Models.Role))]
        public IHttpActionResult DeleteRoleById(int id)
        {
            Role role = RoleServices.DeleteRole(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RoleServices.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleExists(int id)
        {
            return RoleServices.RoleExists(id);
        }
    }
}