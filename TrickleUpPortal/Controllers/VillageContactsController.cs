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
    public class VillageContactsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/VillageContacts
        //public IQueryable<VillageContact> GetVillageContacts()
        //{
        //    return db.VillageContacts;
        //}


        [HttpGet]
        public HttpResponseMessage GetVillageContacts()
        {
            var ApplicationContact = from VillageContact in db.VillageContacts
                                     join Village in db.Villages on VillageContact.VillageId equals Village.Id
                                     join State in db.States on VillageContact.StateId equals State.Id
                                     join District in db.Districts on VillageContact.DistrictId equals District.Id
                                     join Grampanchayat in db.Grampanchayats on VillageContact.GrampanchayatId equals Grampanchayat.Id
                                     select new { VillageContact.Id, VillageContact.VillageId, Village.VillageName, VillageContact.ContactName, VillageContact.ContactNo, VillageContact.Active, VillageContact.StateId, State.StateName, VillageContact.DistrictId, District.DistrictName, VillageContact.GrampanchayatId, Grampanchayat.GrampanchayatName, VillageContact.Designation};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { ApplicationContact }, success = true, error = string.Empty });
        }

        [HttpGet]
        public HttpResponseMessage ContactsByVillage(int VillageId)
        {
            var ApplicationContact = from VillageContact in db.VillageContacts
                                     join Village in db.Villages on VillageContact.VillageId equals Village.Id
                                     join State in db.States on VillageContact.StateId equals State.Id
                                     join District in db.Districts on VillageContact.DistrictId equals District.Id
                                     join Grampanchayat in db.Grampanchayats on VillageContact.GrampanchayatId equals Grampanchayat.Id
                                     where VillageContact.VillageId == VillageId && VillageContact.Active == true
                                     select new { VillageContact.Id, VillageContact.VillageId, Village.VillageName, VillageContact.ContactName, VillageContact.ContactNo, VillageContact.Active, VillageContact.StateId, State.StateName, VillageContact.DistrictId, District.DistrictName, VillageContact.GrampanchayatId, Grampanchayat.GrampanchayatName, VillageContact.Designation };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { ApplicationContact }, success = true, error = string.Empty });
        }

        // GET: api/VillageContacts/5
        [ResponseType(typeof(VillageContact))]
        public IHttpActionResult GetVillageContact(int id)
        {
            VillageContact villageContact = db.VillageContacts.Find(id);
            if (villageContact == null)
            {
                return NotFound();
            }

            return Ok(villageContact);
        }

        // PUT: api/VillageContacts/5
        //[ResponseType(typeof(void))]
        [HttpPost]
        public HttpResponseMessage PutVillageContact(int id, VillageContact villageContact)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != villageContact.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(villageContact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillageContactExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { villageContact }, success = true, error = string.Empty });
        }

        // POST: api/VillageContacts
        //[ResponseType(typeof(VillageContact))]
        [HttpPost]
        public HttpResponseMessage PostVillageContact(VillageContact villageContact)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.VillageContacts.Add(villageContact);
            db.SaveChanges();
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = villageContact.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/VillageContacts/5
        [ResponseType(typeof(VillageContact))]
        public IHttpActionResult DeleteVillageContact(int id)
        {
            VillageContact villageContact = db.VillageContacts.Find(id);
            if (villageContact == null)
            {
                return NotFound();
            }

            db.VillageContacts.Remove(villageContact);
            db.SaveChanges();

            return Ok(villageContact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VillageContactExists(int id)
        {
            return db.VillageContacts.Count(e => e.Id == id) > 0;
        }
    }
}