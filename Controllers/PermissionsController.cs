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
using Permission = UserRolesManagement.DataModels.Permission;

namespace UserRolesManagement.Controllers
{
    public class PermissionsController : ApiController
    {
        public PermissionServices  PermissionServices { get; set; }
        public PermissionsController()
        {
            PermissionServices = new PermissionServices();
        }

        [Route("Permissions")]
        [HttpGet]
        public IHttpActionResult GetPermissions()
        {
            List<Permission> data = PermissionServices.GetAllPermissions();
            return Ok(data);
        }

        [Route("GetPermissionById")]
        [HttpGet]
        [ResponseType(typeof(Models.Permission))]
        public IHttpActionResult GetPermissionById(int id)
        {
            Permission permission = PermissionServices.GetPermission(id);
            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("UpdatePermission")]
        public IHttpActionResult UpdatePermission(int id, Permission permission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                PermissionServices.UpdatePermissionById(id, permission);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionExists(id))
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
        [Route("AddPermission")]
        [ResponseType(typeof(Models.Permission))]
        public IHttpActionResult AddPermission(Permission permission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PermissionServices.AddPermission(permission);
            return Ok(permission);
        }

        [HttpDelete]
        [Route("DeletePermission")]
        [ResponseType(typeof(Models.Permission))]
        public IHttpActionResult DeletePermissionById(int id)
        {
            Permission permission = PermissionServices.DeletePermission(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PermissionServices.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PermissionExists(int id)
        {
            return PermissionServices.PermissionExists(id);
        }
    }
}