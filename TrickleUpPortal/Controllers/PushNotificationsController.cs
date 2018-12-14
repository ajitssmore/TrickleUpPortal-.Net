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
    public class PushNotificationsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/PushNotifications
        //public IQueryable<PushNotification> GetPushNotifications()
        //{
        //    return db.PushNotifications;
        //}
        [HttpGet]
        public HttpResponseMessage GetPushNotifications()
        {
            var PushNotificationData = from PushNotification in db.PushNotifications
                            select new { PushNotification.Id, PushNotification.PushNotificationTitle, PushNotification.PushNotificationBody, PushNotification.PushNotificationData };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { PushNotificationData }, success = true, error = string.Empty });
        }

        // GET: api/PushNotifications/5
        [ResponseType(typeof(PushNotification))]
        public IHttpActionResult GetPushNotification(int id)
        {
            PushNotification pushNotification = db.PushNotifications.Find(id);
            if (pushNotification == null)
            {
                return NotFound();
            }

            return Ok(pushNotification);
        }

        // PUT: api/PushNotifications/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPushNotification(int id, PushNotification pushNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pushNotification.Id)
            {
                return BadRequest();
            }

            db.Entry(pushNotification).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PushNotificationExists(id))
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

        // POST: api/PushNotifications
        [ResponseType(typeof(PushNotification))]
        public IHttpActionResult PostPushNotification(PushNotification pushNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PushNotifications.Add(pushNotification);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pushNotification.Id }, pushNotification);
        }

        // DELETE: api/PushNotifications/5
        [ResponseType(typeof(PushNotification))]
        public IHttpActionResult DeletePushNotification(int id)
        {
            PushNotification pushNotification = db.PushNotifications.Find(id);
            if (pushNotification == null)
            {
                return NotFound();
            }

            db.PushNotifications.Remove(pushNotification);
            db.SaveChanges();

            return Ok(pushNotification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PushNotificationExists(int id)
        {
            return db.PushNotifications.Count(e => e.Id == id) > 0;
        }
    }
}