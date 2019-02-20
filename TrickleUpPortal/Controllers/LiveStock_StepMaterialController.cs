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
    public class LiveStock_StepMaterialController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_StepMaterial
        //public IQueryable<LiveStock_StepMaterial> GetLiveStock_StepMaterial()
        //{
        //    return db.LiveStock_StepMaterial;
        //}

        [HttpGet]
        public HttpResponseMessage GetLiveStock_StepMaterial()
        {
            //return db.LiveStock_Steps;
            var LiveStock_StepMaterials = from LiveStock_StepMaterial in db.LiveStock_StepMaterial
                                          join LiveStock_Step in db.LiveStock_Steps on LiveStock_StepMaterial.LiveStock_StepId equals LiveStock_Step.Id
                                          join LiveStock in db.LiveStocks on LiveStock_Step.LiveStockId equals LiveStock.Id
                                          select new { LiveStock_StepMaterial.Id, LiveStock_StepMaterial.LiveMaterialName, LiveStock_StepMaterial.LiveMaterialDesc, LiveStock_StepMaterial.ImageURL, LiveStock_StepMaterial.Active, LiveStock_StepMaterial.LiveStock_StepId, LiveStock_Step.StepName, LiveStock_Step.LiveStockId, LiveStock.StockName, LiveStock_StepMaterial.Cost, LiveStock_StepMaterial.Quantity, LiveStock_StepMaterial.Quantity_measured, LiveStock_StepMaterial.Cost_measured, LiveStock_StepMaterial.Category};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock_StepMaterials }, success = true, error = string.Empty });
        }

        // GET: api/LiveStock_StepMaterial/5
        [ResponseType(typeof(LiveStock_StepMaterial))]
        public IHttpActionResult GetLiveStock_StepMaterial(int id)
        {
            LiveStock_StepMaterial liveStock_StepMaterial = db.LiveStock_StepMaterial.Find(id);
            if (liveStock_StepMaterial == null)
            {
                return NotFound();
            }

            return Ok(liveStock_StepMaterial);
        }

        // PUT: api/LiveStock_StepMaterial/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStock_StepMaterial(int id, LiveStock_StepMaterial liveStock_StepMaterial)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStock_StepMaterial.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStock_StepMaterial).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStock_StepMaterialExists(id))
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
        public HttpResponseMessage PutLiveStock_StepMaterial(int id, LiveStock_StepMaterial liveStock_StepMaterial)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_StepMaterial.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            try
            {
                LiveStock_StepMaterial LiveStock_StepMaterialData = db.LiveStock_StepMaterial.Where(a => a.Id == liveStock_StepMaterial.Id).FirstOrDefault();
                    LiveStock_StepMaterialData.LiveMaterialName = liveStock_StepMaterial.LiveMaterialName;
                    LiveStock_StepMaterialData.LiveMaterialDesc = liveStock_StepMaterial.LiveMaterialDesc;
                    LiveStock_StepMaterialData.LiveStock_StepId = liveStock_StepMaterial.LiveStock_StepId;
                    LiveStock_StepMaterialData.Quantity = liveStock_StepMaterial.Quantity;
                    LiveStock_StepMaterialData.Cost = liveStock_StepMaterial.Cost;
                    LiveStock_StepMaterialData.Quantity_measured = liveStock_StepMaterial.Quantity_measured;
                    LiveStock_StepMaterialData.Cost_measured = liveStock_StepMaterial.Cost_measured;
                    LiveStock_StepMaterialData.Category = liveStock_StepMaterial.Category;
                    LiveStock_StepMaterialData.Active = liveStock_StepMaterial.Active;

                if (liveStock_StepMaterial.Active == true)
                {
                    LiveStock_StepMaterialData.UpdatedBy = liveStock_StepMaterial.UpdatedBy;
                    LiveStock_StepMaterialData.UpdatedOn = liveStock_StepMaterial.UpdatedOn;
                }
                else if (liveStock_StepMaterial.Active == false)
                {
                    LiveStock_StepMaterialData.ActiveBy = liveStock_StepMaterial.ActiveBy;
                    LiveStock_StepMaterialData.ActiveOn = liveStock_StepMaterial.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_StepMaterialExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_StepMaterial }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_StepMaterial
        //[ResponseType(typeof(LiveStock_StepMaterial))]
        //public IHttpActionResult PostLiveStock_StepMaterial(LiveStock_StepMaterial liveStock_StepMaterial)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStock_StepMaterial.Add(liveStock_StepMaterial);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock_StepMaterial.Id }, liveStock_StepMaterial);
        //}
        [HttpPost]
        public HttpResponseMessage PostLiveStock_StepMaterial(LiveStock_StepMaterial liveStock_StepMaterial)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.LiveStock_StepMaterial.Add(liveStock_StepMaterial);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_StepMaterial.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_StepMaterial/5
        [ResponseType(typeof(LiveStock_StepMaterial))]
        public IHttpActionResult DeleteLiveStock_StepMaterial(int id)
        {
            LiveStock_StepMaterial liveStock_StepMaterial = db.LiveStock_StepMaterial.Find(id);
            if (liveStock_StepMaterial == null)
            {
                return NotFound();
            }

            db.LiveStock_StepMaterial.Remove(liveStock_StepMaterial);
            db.SaveChanges();

            return Ok(liveStock_StepMaterial);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_StepMaterialExists(int id)
        {
            return db.LiveStock_StepMaterial.Count(e => e.Id == id) > 0;
        }

        [HttpPost]
        public HttpResponseMessage UpdateLiveStockImagePath(LiveStock_StepMaterial liveStock_StepMaterial)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStock_StepMaterial LiveStock_StepsMaterialData = db.LiveStock_StepMaterial.Where(a => a.Id == liveStock_StepMaterial.Id).FirstOrDefault();
                LiveStock_StepsMaterialData.ImageURL = liveStock_StepMaterial.ImageURL;

                if (liveStock_StepMaterial.UpdatedBy != null)
                {
                    LiveStock_StepsMaterialData.UpdatedBy = liveStock_StepMaterial.UpdatedBy;
                    LiveStock_StepsMaterialData.UpdatedOn = liveStock_StepMaterial.UpdatedOn;
                }

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_StepMaterialExists(liveStock_StepMaterial.Id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_StepMaterial }, success = true, error = string.Empty });
        }
    }
}