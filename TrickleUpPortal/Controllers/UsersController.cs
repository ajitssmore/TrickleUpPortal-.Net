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
    public class UsersController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Users
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        // GET: api/Users
        public HttpResponseMessage GetAllUsers()
        {
            var result = from User in db.Users
                         join Gender in db.Genders on User.Gender equals Gender.Id
                         join State in db.States on User.State equals State.Id
                         join Role in db.Roles on User.Role equals Role.Id
                         select new { User.Id, User.UserId, User.Name, User.PhoneNumber, User.Age, Gender.GenderName, State.StateName, Role.RoleName};


            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = result, success = true, error = string.Empty });
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public HttpResponseMessage GetUser(int id)
        {
            //User user = db.Users.Find(id);
            //if (user == null)
            //{
            //    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = string.Empty, success = false, error = string.Empty });
            //}

            var result = from User in db.Users
                         join Gender in db.Genders on User.Gender equals Gender.Id
                         join State in db.States on User.State equals State.Id
                         join Role in db.Roles on User.Role equals Role.Id
                         where User.Id == id
                         select new { User.Id, User.UserId, User.Name, User.PhoneNumber, User.Age, Gender.GenderName, State.StateName, Role.RoleName };
                        

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = result, success = true, error = string.Empty });
        }

        // PUT: api/Users/5
        [HttpPost]
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != user.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { user }, success = true, error = string.Empty });
        }

        // POST: api/Users


        [HttpPost]
        //I did changes
        public HttpResponseMessage PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Users.Add(user);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = user.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}