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
    public class UserFCMTokensController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/UserFCMTokens
        public IQueryable<UserFCMToken> GetUserFCMTokens()
        {
            return db.UserFCMTokens;
        }

        // GET: api/UserFCMTokens/5
        [ResponseType(typeof(UserFCMToken))]
        public IHttpActionResult GetUserFCMToken(int id)
        {
            UserFCMToken userFCMToken = db.UserFCMTokens.Find(id);
            if (userFCMToken == null)
            {
                return NotFound();
            }

            return Ok(userFCMToken);
        }

        // PUT: api/UserFCMTokens/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutUserFCMToken(int id, UserFCMToken userFCMToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != userFCMToken.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(userFCMToken).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserFCMTokenExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        [HttpPost]
        public HttpResponseMessage CreateUpdateFCMToken(UserFCMToken userFCMToken)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            int count = db.UserFCMTokens.Where(a => a.UserId == userFCMToken.UserId).Count();
            if (count == 0)
            {
                db.UserFCMTokens.Add(userFCMToken);
                db.SaveChanges();
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userFCMToken }, success = true, error = string.Empty });
            }
            else if (count > 0)
            {
                UserFCMToken UserFCMTokendata = db.UserFCMTokens.Where(a => a.UserId == userFCMToken.UserId).FirstOrDefault();
                UserFCMTokendata.FCMToken = userFCMToken.FCMToken;
                UserFCMTokendata.Registered = userFCMToken.Registered;
                db.SaveChanges();
            } 

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userFCMToken }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage PutUserFCMToken(int UserId, UserFCMToken userFCMToken)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //if (id != userFCMToken.Id)
            //{
            //    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            //}

            //db.Entry(userFCMToken).State = EntityState.Modified;

            try
            {
                UserFCMToken UserFCMTokendata = db.UserFCMTokens.Where(a => a.UserId == UserId).FirstOrDefault();
                UserFCMTokendata.FCMToken = userFCMToken.FCMToken;
                UserFCMTokendata.Registered = userFCMToken.Registered;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFCMTokenExists(UserId))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userFCMToken }, success = true, error = string.Empty });
        }

        // POST: api/UserFCMTokens
        //[ResponseType(typeof(UserFCMToken))]
        //public IHttpActionResult PostUserFCMToken(UserFCMToken userFCMToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.UserFCMTokens.Add(userFCMToken);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = userFCMToken.Id }, userFCMToken);
        //}
        [HttpPost]
        public HttpResponseMessage PostUserFCMToken(UserFCMToken userFCMToken)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            int count = db.UserFCMTokens.Where(a => a.UserId == userFCMToken.UserId).Count();
            if (count > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "FCM Tokan already exists for User", success = false, error = string.Empty });
            }

            db.UserFCMTokens.Add(userFCMToken);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = userFCMToken.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/UserFCMTokens/5
        [ResponseType(typeof(UserFCMToken))]
        public IHttpActionResult DeleteUserFCMToken(int id)
        {
            UserFCMToken userFCMToken = db.UserFCMTokens.Find(id);
            if (userFCMToken == null)
            {
                return NotFound();
            }

            db.UserFCMTokens.Remove(userFCMToken);
            db.SaveChanges();

            return Ok(userFCMToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserFCMTokenExists(int id)
        {
            return db.UserFCMTokens.Count(e => e.Id == id) > 0;
        }
    }
}