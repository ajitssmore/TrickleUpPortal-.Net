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
    public class VillagesController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Villages
        //public IQueryable<Village> GetVillages()
        //{
        //    return db.Villages;
        //}

        [HttpGet]
        public HttpResponseMessage GetVillages()
        {
            var Villagedata = from Village in db.Villages
                              join Grampanchayat in db.Grampanchayats on Village.Grampanchayat equals Grampanchayat.Id
                              join District in db.Districts on Village.District equals District.Id
                              join State in db.States on Village.State equals State.Id
                              select new { Village.Id, Village.VillageName, Village.Grampanchayat, Grampanchayat.GrampanchayatName, Village.District, District.DistrictName, Village.State, State.StateName, Village.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Villagedata }, success = true, error = string.Empty });
        }

        // GET: api/Villages/5
        [ResponseType(typeof(Village))]
        public IHttpActionResult GetVillage(int id)
        {
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return NotFound();
            }

            return Ok(village);
        }

        // PUT: api/Villages/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public HttpResponseMessage PutVillage(int id, Village village)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != village.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var villageData = db.Villages.Where(q => q.VillageName.ToUpper() == village.VillageName.ToUpper()).Any() ? db.Villages.Where(p => p.VillageName.ToUpper() == village.VillageName.ToUpper()).First() : null;
            if (villageData != null && villageData.Id != village.Id)
            {
                if (db.Villages.Any(p => p.VillageName.ToUpper() == village.VillageName.ToUpper()))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "village Name already exists" });
                }
            }
            else
            {
                try
                {
                    Village VillageUpdateData = db.Villages.Where(a => a.Id == village.Id).FirstOrDefault();
                    VillageUpdateData.VillageName = village.VillageName;
                    VillageUpdateData.State = village.State;
                    VillageUpdateData.District = village.District;
                    VillageUpdateData.Grampanchayat = village.Grampanchayat;
                    VillageUpdateData.UpdatedBy = village.UpdatedBy;
                    VillageUpdateData.UpdatedOn = village.UpdatedOn;
                    VillageUpdateData.Active = village.Active;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VillageExists(id))
                    {
                        return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { village }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ActiveDeactiveVillage(int id, Village village)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != village.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            try
            {
                Village VillageUpdateData = db.Villages.Where(a => a.Id == village.Id).FirstOrDefault();
                VillageUpdateData.ActiveBy = village.ActiveBy;
                VillageUpdateData.ActiveOn = village.ActiveOn;
                VillageUpdateData.Active = village.Active;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VillageExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { village }, success = true, error = string.Empty });
        }

        // POST: api/Villages
        //[ResponseType(typeof(Village))]
        [HttpPost]
        public HttpResponseMessage PostVillage(Village village)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            var DataFound = (from Villagedata in db.Villages
                             where Villagedata.VillageName.ToUpper() == village.VillageName.ToUpper()
                             select Villagedata.VillageName).SingleOrDefault();

            if (DataFound == null)
            {
                db.Villages.Add(village);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "village Name already exists" });
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = village.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Villages/5
        [ResponseType(typeof(Village))]
        public IHttpActionResult DeleteVillage(int id)
        {
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return NotFound();
            }

            db.Villages.Remove(village);
            db.SaveChanges();

            return Ok(village);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VillageExists(int id)
        {
            return db.Villages.Count(e => e.Id == id) > 0;
        }
    }
}