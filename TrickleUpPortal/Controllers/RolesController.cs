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
                         select new { role.Id, role.RoleName, role.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Roles }, success = true, error = string.Empty });
        }

        // GET: api/Roles/5
        [ResponseType(typeof(Role))]
        public IHttpActionResult GetRole(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
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

            db.Entry(role).State = EntityState.Modified;

            try
            {
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

            db.Roles.Add(role);
            db.SaveChanges();

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