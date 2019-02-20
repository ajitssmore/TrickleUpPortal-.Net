using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class LiveStocksController : ApiController
    {
        CommonController comObj = new CommonController();
        string LanguageName;
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/LiveStocks
        //public IQueryable<LiveStock> GetLiveStocks()
        //{
        //    return db.LiveStocks;
        //}

        public HttpResponseMessage GetLiveStocks()
        {
            var LiveStock = from LiveStockdata in db.LiveStocks
                            select new { LiveStockdata.Id, LiveStockdata.StockName, LiveStockdata.Active, LiveStockdata.ImageURL};
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }

        // GET: api/LiveStocks/5
        [ResponseType(typeof(LiveStock))]
        public IHttpActionResult GetLiveStock(int id)
        {
            LiveStock liveStock = db.LiveStocks.Find(id);
            if (liveStock == null)
            {
                return NotFound();
            }

            return Ok(liveStock);
        }

        // PUT: api/LiveStocks/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutLiveStock(int id, LiveStock liveStock)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != liveStock.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(liveStock).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!LiveStockExists(id))
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
        public HttpResponseMessage PutLiveStock(int id, LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != liveStock.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //db.Entry(liveStock).State = EntityState.Modified;

            try
            {
                LiveStock LiveStockData = db.LiveStocks.Where(a => a.Id == liveStock.Id).FirstOrDefault();
                LiveStockData.StockName = liveStock.StockName;
                LiveStockData.Active = liveStock.Active;
                if (liveStock.Active == true)
                {
                    LiveStockData.UpdatedBy = liveStock.UpdatedBy;
                    LiveStockData.UpdatedOn = liveStock.UpdatedOn;
                }
                else if (liveStock.Active == false)
                {
                    LiveStockData.ActiveBy = liveStock.ActiveBy;
                    LiveStockData.ActiveOn = liveStock.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock }, success = true, error = string.Empty });
        }


        // POST: api/LiveStocks
        //[ResponseType(typeof(LiveStock))]
        //public IHttpActionResult PostLiveStock(LiveStock liveStock)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.LiveStocks.Add(liveStock);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = liveStock.Id }, liveStock);
        //}

        [HttpPost]
        public HttpResponseMessage PostLiveStock(LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var DataFound = (from LiveStocksdata in db.LiveStocks
                             where LiveStocksdata.StockName.ToUpper() == liveStock.StockName.ToUpper()
                             select LiveStocksdata.StockName).SingleOrDefault();

            if (DataFound == null)
            {
                db.LiveStocks.Add(liveStock);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Stock Name already exists" });
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = liveStock.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/LiveStocks/5
        [ResponseType(typeof(LiveStock))]
        public IHttpActionResult DeleteLiveStock(int id)
        {
            LiveStock liveStock = db.LiveStocks.Find(id);
            if (liveStock == null)
            {
                return NotFound();
            }

            db.LiveStocks.Remove(liveStock);
            db.SaveChanges();

            return Ok(liveStock);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LiveStockExists(int id)
        {
            return db.LiveStocks.Count(e => e.Id == id) > 0;
        }

        public HttpResponseMessage GetLiveStocksProcessData(int LangCode)
        {
            LanguageName = comObj.fetchLang(LangCode);
            List<LiveStock> LiveStock = db.LiveStocks.ToList();
            var removeLiveStock = LiveStock.Where(a => a.Active == false);
            foreach (var removeLiveStockData in removeLiveStock.ToList())
            {
                LiveStock.Remove(removeLiveStockData);
            }

            if (LiveStock.Count > 0)
            {
                switch (LanguageName)
                {
                    case "Hindi":
                        foreach (LiveStock LiveData in LiveStock)
                        {
                            var removeLiveStockBreed = LiveData.LiveStockBreeds.Where(a => a.Active == false);
                            foreach (var removeLiveStockBreedData in removeLiveStockBreed.ToList())
                            {
                                LiveData.LiveStockBreeds.Remove(removeLiveStockBreedData);
                            }

                            LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Hindi(LiveData.StockName) : string.Empty;
                            LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                            foreach (var LiveStockBreed in LiveData.LiveStockBreeds)
                            {
                                var removeLiveStockBreedCategory = LiveStockBreed.LiveStock_BreedCategory.Where(a => a.Active == false);
                                foreach (var removeLiveStockBreedCategoryData in removeLiveStockBreedCategory.ToList())
                                {
                                    LiveStockBreed.LiveStock_BreedCategory.Remove(removeLiveStockBreedCategoryData);
                                }
                                LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Hindi(LiveStockBreed.BreedName) : string.Empty;
                                LiveStockBreed.AudioURL = comObj.fetchAudioPahtLiveStockBreed(LiveStockBreed.Id, LangCode);
                                foreach (var BreedCategory in LiveStockBreed.LiveStock_BreedCategory)
                                {
                                    BreedCategory.CategoryName = !string.IsNullOrEmpty(BreedCategory.CategoryName) ? comObj.GetResxNameByValue_Hindi(BreedCategory.CategoryName) : string.Empty;
                                    BreedCategory.AudioURL = comObj.fetchAudioPahtLiveStockBreedCategory(BreedCategory.Id, LangCode);
                                }
                            }

                            var removeLiveStock_Steps = LiveData.LiveStock_Steps.Where(a => a.Active == false);
                            foreach (var removeLiveStock_StepsData in removeLiveStock_Steps.ToList())
                            {
                                LiveData.LiveStock_Steps.Remove(removeLiveStock_StepsData);
                            }

                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                            {
                                StepData.StepName = !string.IsNullOrEmpty(StepData.StepName) ? comObj.GetResxNameByValue_Hindi(StepData.StepName) : string.Empty;
                                StepData.StepAudioTitleURL = comObj.fetchAudioPahtLiveStockSteps(StepData.Id, LangCode);
                                StepData.StepDescription = !string.IsNullOrEmpty(StepData.StepDescription) ? comObj.GetResxNameByValue_Hindi(StepData.StepDescription) : string.Empty;

                                var removeLiveStock_StepMaterial = StepData.LiveStock_StepMaterial.Where(a => a.Active == false);
                                foreach (var removeLiveStock_StepMaterialData in removeLiveStock_StepMaterial.ToList())
                                {
                                    StepData.LiveStock_StepMaterial.Remove(removeLiveStock_StepMaterialData);
                                }
                                foreach (var LiveStock_StepMaterials in StepData.LiveStock_StepMaterial)
                                {
                                    LiveStock_StepMaterials.LiveMaterialName = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialName) ? comObj.GetResxNameByValue_Hindi(LiveStock_StepMaterials.LiveMaterialName) : string.Empty;
                                    LiveStock_StepMaterials.LiveMaterialDesc = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialDesc) ? comObj.GetResxNameByValue_Hindi(LiveStock_StepMaterials.LiveMaterialDesc) : string.Empty;
                                    LiveStock_StepMaterials.TitleAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Title");
                                    LiveStock_StepMaterials.DescriptionAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Description");
                                }
                                List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                StepData.Adult = AdultData;
                                StepData.Child = ChildData;
                            }
                        }
                        break;
                    case "English":
                            foreach (LiveStock LiveData in LiveStock)
                            {
                                LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                                foreach (var LiveStockBreed in LiveData.LiveStockBreeds)
                                {
                                    LiveStockBreed.AudioURL = comObj.fetchAudioPahtLiveStockBreed(LiveStockBreed.Id, LangCode);
                                    foreach (var BreedCategory in LiveStockBreed.LiveStock_BreedCategory)
                                    {
                                        BreedCategory.AudioURL = comObj.fetchAudioPahtLiveStockBreedCategory(BreedCategory.Id, LangCode);
                                    }
                                }

                                foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                                        {
                                            StepData.StepAudioTitleURL = comObj.fetchAudioPahtLiveStockSteps(StepData.Id, LangCode);

                                            foreach (var LiveStock_StepMaterials in StepData.LiveStock_StepMaterial)
                                            {
                                                LiveStock_StepMaterials.TitleAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Title");
                                                LiveStock_StepMaterials.DescriptionAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Description");
                                            }
                                            List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                                StepData.Adult = AdultData;
                                                StepData.Child = ChildData;
                                            }
                                    }
                        break;
                    case "Oriya":
                        foreach (LiveStock LiveData in LiveStock)
                        {
                            LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Oriya(LiveData.StockName) : string.Empty;
                            LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                            foreach (var LiveStockBreed in LiveData.LiveStockBreeds)
                            {
                                LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Oriya(LiveStockBreed.BreedName) : string.Empty;
                                LiveStockBreed.AudioURL = comObj.fetchAudioPahtLiveStockBreed(LiveStockBreed.Id, LangCode);
                                foreach (var BreedCategory in LiveStockBreed.LiveStock_BreedCategory)
                                {
                                    BreedCategory.CategoryName = !string.IsNullOrEmpty(BreedCategory.CategoryName) ? comObj.GetResxNameByValue_Oriya(BreedCategory.CategoryName) : string.Empty;
                                    BreedCategory.AudioURL = comObj.fetchAudioPahtLiveStockBreedCategory(BreedCategory.Id, LangCode);
                                }
                            }

                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                            {
                                StepData.StepName = !string.IsNullOrEmpty(StepData.StepName) ? comObj.GetResxNameByValue_Oriya(StepData.StepName) : string.Empty;
                                StepData.StepAudioTitleURL = comObj.fetchAudioPahtLiveStockSteps(StepData.Id, LangCode);
                                StepData.StepDescription = !string.IsNullOrEmpty(StepData.StepDescription) ? comObj.GetResxNameByValue_Oriya(StepData.StepDescription) : string.Empty;
                                foreach (var LiveStock_StepMaterials in StepData.LiveStock_StepMaterial)
                                {
                                    LiveStock_StepMaterials.LiveMaterialName = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialName) ? comObj.GetResxNameByValue_Oriya(LiveStock_StepMaterials.LiveMaterialName) : string.Empty;
                                    LiveStock_StepMaterials.LiveMaterialDesc = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialDesc) ? comObj.GetResxNameByValue_Oriya(LiveStock_StepMaterials.LiveMaterialDesc) : string.Empty;
                                    LiveStock_StepMaterials.TitleAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Title");
                                    LiveStock_StepMaterials.DescriptionAudioURL = comObj.fetchAudioPahtLiveStockStepsMaterial(LiveStock_StepMaterials.Id, LangCode, "Description");
                                }
                                List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                StepData.Adult = AdultData;
                                StepData.Child = ChildData;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }

        public HttpResponseMessage GetLiveStocksProcessDataNew(int LangCode)
        {
            List<LiveStock> LiveStock = db.LiveStocks.ToList();
            LanguageName = comObj.fetchLang(LangCode);
            if (LiveStock.Count > 0)
            {
                switch (LanguageName)
                {
                    case "Hindi":
                        foreach (LiveStock LiveData in LiveStock)
                        {
                            LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Hindi(LiveData.StockName) : string.Empty;
                            LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                            foreach (var LiveStockBreed in LiveData.LiveStockBreeds)
                            {
                                LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Hindi(LiveStockBreed.BreedName) : string.Empty;
                                LiveStockBreed.AudioURL = comObj.fetchAudioPahtLiveStockBreed(LiveStockBreed.Id, LangCode);
                                foreach (var BreedCategory in LiveStockBreed.LiveStock_BreedCategory)
                                {
                                    BreedCategory.CategoryName = !string.IsNullOrEmpty(BreedCategory.CategoryName) ? comObj.GetResxNameByValue_Hindi(BreedCategory.CategoryName) : string.Empty;
                                    BreedCategory.AudioURL = comObj.fetchAudioPahtLiveStockBreedCategory(BreedCategory.Id, LangCode);
                                }
                            }

                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                            {
                                StepData.StepName = !string.IsNullOrEmpty(StepData.StepName) ? comObj.GetResxNameByValue_Hindi(StepData.StepName) : string.Empty;
                                StepData.StepDescription = !string.IsNullOrEmpty(StepData.StepDescription) ? comObj.GetResxNameByValue_Hindi(StepData.StepDescription) : string.Empty;
                                foreach (var LiveStock_StepMaterials in StepData.LiveStock_StepMaterial)
                                {
                                    LiveStock_StepMaterials.LiveMaterialName = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialName) ? comObj.GetResxNameByValue_Hindi(LiveStock_StepMaterials.LiveMaterialName) : string.Empty;
                                    LiveStock_StepMaterials.LiveMaterialDesc = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialDesc) ? comObj.GetResxNameByValue_Hindi(LiveStock_StepMaterials.LiveMaterialDesc) : string.Empty;
                                }
                                List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                StepData.Adult = AdultData;
                                StepData.Child = ChildData;
                            }
                        }
                        break;
                    case "English":
                        foreach (LiveStock LiveData in LiveStock)
                        {
                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                            {
                                List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                StepData.Adult = AdultData;
                                StepData.Child = ChildData;
                            }
                        }
                        break;
                    case "Oriya":
                        foreach (LiveStock LiveData in LiveStock)
                        {
                            LiveData.StockName = !string.IsNullOrEmpty(LiveData.StockName) ? comObj.GetResxNameByValue_Oriya(LiveData.StockName) : string.Empty;
                            LiveData.AudioURL = comObj.fetchAudioPahtLiveStock(LiveData.Id, LangCode);
                            foreach (var LiveStockBreed in LiveData.LiveStockBreeds)
                            {
                                LiveStockBreed.BreedName = !string.IsNullOrEmpty(LiveStockBreed.BreedName) ? comObj.GetResxNameByValue_Oriya(LiveStockBreed.BreedName) : string.Empty;
                                LiveStockBreed.AudioURL = comObj.fetchAudioPahtLiveStockBreed(LiveStockBreed.Id, LangCode);
                                foreach (var BreedCategory in LiveStockBreed.LiveStock_BreedCategory)
                                {
                                    BreedCategory.CategoryName = !string.IsNullOrEmpty(BreedCategory.CategoryName) ? comObj.GetResxNameByValue_Oriya(BreedCategory.CategoryName) : string.Empty;
                                    BreedCategory.AudioURL = comObj.fetchAudioPahtLiveStockBreedCategory(BreedCategory.Id, LangCode);
                                }
                            }

                            foreach (LiveStock_Steps StepData in LiveData.LiveStock_Steps)
                            {
                                StepData.StepName = !string.IsNullOrEmpty(StepData.StepName) ? comObj.GetResxNameByValue_Oriya(StepData.StepName) : string.Empty;
                                StepData.StepDescription = !string.IsNullOrEmpty(StepData.StepDescription) ? comObj.GetResxNameByValue_Oriya(StepData.StepDescription) : string.Empty;
                                foreach (var LiveStock_StepMaterials in StepData.LiveStock_StepMaterial)
                                {
                                    LiveStock_StepMaterials.LiveMaterialName = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialName) ? comObj.GetResxNameByValue_Oriya(LiveStock_StepMaterials.LiveMaterialName) : string.Empty;
                                    LiveStock_StepMaterials.LiveMaterialDesc = !string.IsNullOrEmpty(LiveStock_StepMaterials.LiveMaterialDesc) ? comObj.GetResxNameByValue_Oriya(LiveStock_StepMaterials.LiveMaterialDesc) : string.Empty;
                                }
                                List<LiveStock_StepMaterial> AdultData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Adult").ToList();
                                List<LiveStock_StepMaterial> ChildData = StepData.LiveStock_StepMaterial.Where(a => a.Category.ToString() == "Child").ToList();
                                StepData.Adult = AdultData;
                                StepData.Child = ChildData;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { LiveStock }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage UpdateLiveStockImagePath(LiveStock liveStock)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                LiveStock LiveStockData = db.LiveStocks.Where(a => a.Id == liveStock.Id).FirstOrDefault();
                LiveStockData.ImageURL = liveStock.ImageURL;

                if (liveStock.UpdatedBy != null)
                {
                    LiveStockData.UpdatedBy = liveStock.UpdatedBy;
                    LiveStockData.UpdatedOn = liveStock.UpdatedOn;
                }

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStockExists(liveStock.Id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { liveStock }, success = true, error = string.Empty });
        }
    }
}