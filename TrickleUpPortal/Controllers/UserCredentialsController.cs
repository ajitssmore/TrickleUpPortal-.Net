using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class UserCredentialsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/UserCredentials
        public IQueryable<UserCredential> GetUserCredentials()
        {
            return db.UserCredentials;
        }

        // GET: api/UserCredentials/5
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult GetUserCredential(int id)
        {
            UserCredential userCredential = db.UserCredentials.Find(id);
            if (userCredential == null)
            {
                return NotFound();
            }

            return Ok(userCredential);
        }

        // GET: api/UserCredentials/AuthenticateUser/
        [HttpPost]
       public HttpResponseMessage AuthenticateUser([FromBody]UserCredential userCredential)
        {
            try
            {
                //UserCredential userCredential = (UserCredential)db.UserCredentials.Where(a => a.UserName == userName && a.Password == userPassword).SingleOrDefault();

                var result = from UserCredential in db.UserCredentials
                             join User in db.Users on UserCredential.Id equals User.Id
                             where UserCredential.UserName == userCredential.UserName && UserCredential.Password == userCredential.Password
                             select new { UserCredential.Id, UserCredential.UserName, User.PhoneNumber};

                dynamic UserVerification = new ExpandoObject();
                //List<Object> dataList = new List<Object>();
                var Isauthenticated = false;
                foreach (var item in result)
                {
                    Isauthenticated = true;
                    UserVerification.Id = item.Id;
                    UserVerification.UserName = item.UserName;
                    UserVerification.PhoneNumber = item.PhoneNumber;
                    UserVerification.authenticated = true;
                }
                if (Isauthenticated == false)
                {
                    //return NotFound();
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = "The UserName or Password is Incorrect." });
                }
                else
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = UserVerification, success = true, error = (string)null });
                }
                
            }
            catch (Exception ex)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = (string)null, success = true, error = ex.Message });
            }

        }

        // PUT: api/UserCredentials/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserCredential(int id, UserCredential userCredential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userCredential.Id)
            {
                return BadRequest();
            }

            db.Entry(userCredential).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCredentialExists(id))
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

        // POST: api/UserCredentials
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult PostUserCredential(UserCredential userCredential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserCredentials.Add(userCredential);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserCredentialExists(userCredential.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userCredential.Id }, userCredential);
        }

        // DELETE: api/UserCredentials/5
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult DeleteUserCredential(int id)
        {
            UserCredential userCredential = db.UserCredentials.Find(id);
            if (userCredential == null)
            {
                return NotFound();
            }

            db.UserCredentials.Remove(userCredential);
            db.SaveChanges();

            return Ok(userCredential);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserCredentialExists(int id)
        {
            return db.UserCredentials.Count(e => e.Id == id) > 0;
        }
    }
}