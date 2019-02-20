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
    public class LiveStock_BreedCategoryController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStock_BreedCategory
        //public IQueryable<LiveStock_BreedCategory> GetLiveStock_BreedCategory()
        //{
        //    return db.LiveStock_BreedCategory;
        //}
        [HttpGet]
        public HttpResponseMessage GetLiveStock_BreedCategory()
        {
            //return db.LiveStock_BreedCategory;
            var LiveStockBreedsCategory = from LiveStockBreedCategorys in db.LiveStock_BreedCategory
                                          join LiveStockBreed in db.LiveStockBreeds on LiveStockBreedCategorys.BreedId equals LiveStockBreed.Id
                                          join LiveStock in db.LiveStocks on LiveStockBreed.LiveStockId equals LiveStock.Id
                                          select new { LiveStockBreedCategorys.Id, LiveStockBreedCategorys.CategoryName, LiveStockBreedCategorys.BreedId, LiveStockBreed.BreedName, LiveStockBreedCategorys.ImageURL, LiveStockBreedCategorys.Active, LiveStock.StockName, LiveStockBreed.LiveStockId, LiveStockBreedCategorys.Quantity, LiveStockBreedCategorys.Rate, LiveStockBreedCategorys.Units };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStockBreedsCategory }, success = true, error = string.Empty });
        }

        // GET: api/LiveStock_BreedCategory/5
        [ResponseType(typeof(LiveStock_BreedCategory))]
        public IHttpActionResult GetLiveStock_BreedCategory(int id)
        {
            LiveStock_BreedCategory liveStock_BreedCategory = db.LiveStock_BreedCategory.Find(id);
            if (liveStock_BreedCategory == null)
            {
                return NotFound();
            }

            return Ok(liveStock_BreedCategory);
        }

        // PUT: api/LiveStock_BreedCategory/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStock_BreedCategory(int id, LiveStock_BreedCategory liveStock_BreedCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStock_BreedCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStock_BreedCategory).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStock_BreedCategoryExists(id))
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
        public HttpResponseMessage PutLiveStock_BreedCategory(int id, LiveStock_BreedCategory liveStock_BreedCategory)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock_BreedCategory.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStock_BreedCategory LiveStockBreedCategoryData = db.LiveStock_BreedCategory.Where(a => a.Id == liveStock_BreedCategory.Id).FirstOrDefault();
                LiveStockBreedCategoryData.CategoryName = liveStock_BreedCategory.CategoryName;
                LiveStockBreedCategoryData.BreedId = liveStock_BreedCategory.BreedId;
                LiveStockBreedCategoryData.Rate = liveStock_BreedCategory.Rate;
                LiveStockBreedCategoryData.Units = liveStock_BreedCategory.Units;
                LiveStockBreedCategoryData.Quantity = liveStock_BreedCategory.Quantity;
                LiveStockBreedCategoryData.Active = liveStock_BreedCategory.Active;

                if (liveStock_BreedCategory.Active == true)
                {
                    LiveStockBreedCategoryData.UpdatedBy = liveStock_BreedCategory.UpdatedBy;
                    LiveStockBreedCategoryData.UpdatedOn = liveStock_BreedCategory.UpdatedOn;
                }
                else if (liveStock_BreedCategory.Active == false)
                {
                    LiveStockBreedCategoryData.ActiveBy = liveStock_BreedCategory.ActiveBy;
                    LiveStockBreedCategoryData.ActiveOn = liveStock_BreedCategory.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStock_BreedCategoryExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock_BreedCategory }, success = true, error = string.Empty });
        }

        // POST: api/LiveStock_BreedCategory
        //[ResponseType(typeof(LiveStock_BreedCategory))]
        //public IHttpActionResult PostLiveStock_BreedCategory(LiveStock_BreedCategory liveStock_BreedCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStock_BreedCategory.Add(liveStock_BreedCategory);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock_BreedCategory.Id }, liveStock_BreedCategory);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStock_BreedCategory(LiveStock_BreedCategory liveStock_BreedCategory)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.LiveStock_BreedCategory.Add(liveStock_BreedCategory);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock_BreedCategory.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStock_BreedCategory/5
        [ResponseType(typeof(LiveStock_BreedCategory))]
        public IHttpActionResult DeleteLiveStock_BreedCategory(int id)
        {
            LiveStock_BreedCategory liveStock_BreedCategory = db.LiveStock_BreedCategory.Find(id);
            if (liveStock_BreedCategory == null)
            {
                return NotFound();
            }

            db.LiveStock_BreedCategory.Remove(liveStock_BreedCategory);
            db.SaveChanges();

            return Ok(liveStock_BreedCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStock_BreedCategoryExists(int id)
        {
            return db.LiveStock_BreedCategory.Count(e => e.Id == id) > 0;
        }
    }
}