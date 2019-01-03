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
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class RolesController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Roles
        //public IQueryable<Role> GetRoles()
        //{
        //    return db.Roles;
        //}

        [HttpGet]
        public HttpResponseMessage GetRoles()
        {
            var Roles = from role in db.Roles
                         select new { role.Id, role.RoleId, role.RoleName, role.Active, role.Description };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Roles }, success = true, error = string.Empty });
        }

        // GET: api/Roles/5
        //[ResponseType(typeof(Role))]
        [HttpGet]
        public HttpResponseMessage GetSepecificRole(int id)
        {
            //Role role = db.Roles.Find(id);
            var Roles = from role in db.Roles
                        where role.Id == id
                        select new { role.Id, role.RoleId, role.RoleName, role.Active, role.UpdatedBy, role.UpdatedOn, role.ActiveBy, role.ActiveOn };

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Roles }, success = true, error = string.Empty });
        }

        // PUT: api/Roles/5
        [HttpPost]
        public HttpResponseMessage PutRole(int id, Role role)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != role.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var roleData = db.Roles.Where(q => q.RoleName.ToUpper() == role.RoleName.ToUpper()).Any() ? db.Roles.Where(p => p.RoleName.ToUpper() == role.RoleName.ToUpper()).First() : null;
            if (roleData != null && roleData.Id != role.Id)
            {
                if (db.Roles.Any(p => p.RoleName.ToUpper() == role.RoleName.ToUpper()))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Role Name already exists" });
                }
            }
            else
            {
                try
                {
                    Role RoleUpdateData = db.Roles.Where(a => a.Id == role.Id).FirstOrDefault();
                    RoleUpdateData.RoleName = role.RoleName;
                    RoleUpdateData.UpdatedBy = role.UpdatedBy;
                    RoleUpdateData.UpdatedOn = role.UpdatedOn;
                    RoleUpdateData.Active = role.Active;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(id))
                    {
                        return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { role }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ActiveDeactiveRole(int id, Role role)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != role.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            try
            {
                Role RoleUpdateData = db.Roles.Where(a => a.Id == role.Id).FirstOrDefault();
                RoleUpdateData.ActiveBy = role.ActiveBy;
                RoleUpdateData.ActiveOn = role.ActiveOn;
                RoleUpdateData.Active = role.Active;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { role }, success = true, error = string.Empty });
        }
        // POST: api/Roles
        [HttpPost]
        public HttpResponseMessage PostRole(Role role)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var DataFound = (from Roledata in db.Roles
                             where Roledata.RoleName.ToUpper() == role.RoleName.ToUpper()
                             select Roledata.RoleName).SingleOrDefault();

            if (DataFound == null)
            {
                db.Roles.Add(role);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Role Name already exists" });
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = role.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Roles/5
        //[ResponseType(typeof(Role))]
        //public IHttpActionResult DeleteRole(int id)
        //{
        //    Role role = db.Roles.Find(id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Roles.Remove(role);
        //    db.SaveChanges();

        //    return Ok(role);
        //}

        [HttpGet]
        public IHttpActionResult DeativeRole(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);
            db.SaveChanges();

            return Ok(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleExists(int id)
        {
            return db.Roles.Count(e => e.Id == id) > 0;
        }
    }
}