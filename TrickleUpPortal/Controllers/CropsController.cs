using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;
using System.Resources;
using System.Collections;

namespace TrickleUpPortal.Controllers
{
    public class CropsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        CommonController comObj = new CommonController();
        string LanguageName;
        public object serializer { get; private set; }

        // GET: api/Crops
        public IQueryable<Crop> GetCropsdata()
        {
            return db.Crops;
        }

        [HttpGet]
        public HttpResponseMessage GetCrops(int langCode)
        {
            LanguageName = comObj.fetchLang(langCode);
            var results = from Cropdata in db.Crops
                          where Cropdata.Active == true
                          select new { Cropdata.Id, Cropdata.CropName, Cropdata.FilePath, Cropdata.Ready, Cropdata.Active, Cropdata.AliasName};
            List<Cropdata> Crops = new List<Cropdata>();
            foreach (var item in results)
            {
                Cropdata cropObj = new Cropdata();
                switch (LanguageName)
                {
                    case "Hindi":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName!=null ? comObj.GetResxNameByValue_Hindi(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath!=null ? item.FilePath : string.Empty;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        cropObj.AliasName = item.AliasName;
                        Crops.Add(cropObj);
                        break;
                    case "English":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName;
                        cropObj.FilePath = item.FilePath;
                        cropObj.Ready = item.Ready!= null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        cropObj.AliasName = item.AliasName;
                        Crops.Add(cropObj);
                        break;
                    case "Oriya":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName != null ? comObj.GetResxNameByValue_Oriya(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        cropObj.AliasName = item.AliasName;
                        Crops.Add(cropObj);
                        break;
                    case "Santhali":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName != null ? comObj.GetResxNameByValue_Hindi(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath != null ? item.FilePath : string.Empty;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        cropObj.AliasName = item.AliasName;
                        Crops.Add(cropObj);
                        break;
                    case "Ho":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName != null ? comObj.GetResxNameByValue_Hindi(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath != null ? item.FilePath : string.Empty;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        cropObj.AliasName = item.AliasName;
                        Crops.Add(cropObj);
                        break;
                    default:
                        break;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Crops }, success = true, error = string.Empty });
        }

        [HttpGet]
        public HttpResponseMessage GetCropsPortal(int langCode)
        {
            LanguageName = comObj.fetchLang(langCode);
            
            var results = from Cropdata in db.Crops
                          select new { Cropdata.Id, Cropdata.CropName, Cropdata.FilePath, Cropdata.Ready, Cropdata.Active };
            List<Cropdata> Crops = new List<Cropdata>();
            foreach (var item in results)
            {
                Cropdata cropObj = new Cropdata();
                switch (LanguageName)
                {
                    case "Hindi":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName != null ? comObj.GetResxNameByValue_Hindi(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath != null ? item.FilePath : string.Empty;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        Crops.Add(cropObj);
                        break;
                    case "English":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName;
                        cropObj.FilePath = item.FilePath;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        Crops.Add(cropObj);
                        break;
                    case "Oriya":
                        cropObj.Id = item.Id;
                        cropObj.CropName = item.CropName != null ? comObj.GetResxNameByValue_Oriya(item.CropName) : string.Empty;
                        cropObj.FilePath = item.FilePath;
                        cropObj.Ready = item.Ready != null ? (bool)item.Ready : false;
                        cropObj.AudioTitle_Path = comObj.fetchAudioPahtCrops(item.Id, langCode);
                        Crops.Add(cropObj);
                        break;
                    default:
                        break;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Crops }, success = true, error = string.Empty });
        }

        public class Cropdata
        {
            public int Id { get; set; }
            public string CropName { get; set; }
            public string FilePath { get; set; }
            public bool Ready { get; set; }
            public string AudioTitle_Path { get; set; }
            public string AliasName { get; set; }
        }

        //// GET: api/Crops/5
        //[ResponseType(typeof(Crop))]
        //public IHttpActionResult GetCrop(int id)
        //{
        //    Crop crop = db.Crops.Find(id);
        //    if (crop == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(crop);
        //}

        //[HttpGet]
        //public HttpResponseMessage GetCropStepMaterialData(int id)
        //{
        //    try
        //    {
        //        Crop crop = db.Crops.Find(id);
                
        //        if (crop == null)
        //        {
        //            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = "No Data" });
        //        }
        //        else
        //        {
        //            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = crop, success = true, error = (string)null });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = (string)null, success = true, error = ex.Message });
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage GetCropStepMaterialData(int id, int langId)
        {
            try
            {
                LanguageName = comObj.fetchLang(langId);
                Crop crop = db.Crops.Find(id);

                var StepData = crop.Cultivation_Steps.Where(a => a.Active == false);
                foreach (var removeStepData in StepData.ToList())
                {
                    crop.Cultivation_Steps.Remove(removeStepData);
                }

                foreach (var StepsData in crop.Cultivation_Steps)
                {
                    var matraislData = StepsData.CropSteps_Material.Where(b => b.Active == false);
                    foreach (var removematraisl in matraislData.ToList())
                    {
                        StepsData.CropSteps_Material.Remove(removematraisl);
                    }
                }
                
                switch (LanguageName)
                {
                    case "Hindi":
                        crop.CropName = crop.CropName != null ? comObj.GetResxNameByValue_Hindi(crop.CropName) : string.Empty;
                        crop.Title_Audio = comObj.fetchAudioPahtCrops(id, langId);
                        if (crop.Cultivation_Steps.Count > 0)
                        {
                            foreach (var item in crop.Cultivation_Steps)
                            {
                                item.Step_Name = item.Step_Name != null ? comObj.GetResxNameByValue_Hindi(item.Step_Name) : string.Empty;
                                item.Step_Description = item.Step_Description != null ? comObj.GetResxNameByValue_Hindi(item.Step_Description) : string.Empty;
                                item.Title_Audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Title");
                                item.Description_audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Description");
                                if (item.CropSteps_Material.Count > 0)
                                {
                                    foreach (var itemMat in item.CropSteps_Material)
                                    {
                                        itemMat.Material_Name = itemMat.Material_Name != null ? comObj.GetResxNameByValue_Hindi(itemMat.Material_Name) : string.Empty;
                                        itemMat.Audio_Path = comObj.fetchAudioPahtMaterials(itemMat.Id, langId);
                                        itemMat.Image_Path = itemMat.Image_Path != null ? itemMat.Image_Path : string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "There are no steps and Material for this Crop" });
                        }
                        break;
                    case "English":
                        crop.CropName = crop.CropName;
                        crop.Title_Audio = comObj.fetchAudioPahtCrops(id, langId);
                        if (crop.Cultivation_Steps.Count > 0)
                        {
                            foreach (var item in crop.Cultivation_Steps)
                            {
                                item.Title_Audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Title");
                                item.Description_audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Description");
                                if (item.CropSteps_Material.Count > 0)
                                {
                                    foreach (var itemMat in item.CropSteps_Material)
                                    {
                                        itemMat.Audio_Path = comObj.fetchAudioPahtMaterials(itemMat.Id, langId);
                                        itemMat.Image_Path = itemMat.Image_Path != null ? itemMat.Image_Path : string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "There are no steps and Material for this Crop" });
                        }
                        break;
                    case "Oriya":
                        crop.CropName = crop.CropName != null ? comObj.GetResxNameByValue_Oriya(crop.CropName) : string.Empty;
                        crop.Title_Audio = comObj.fetchAudioPahtCrops(id, langId);
                        if (crop.Cultivation_Steps.Count > 0)
                        {
                            foreach (var item in crop.Cultivation_Steps)
                            {
                                item.Step_Name = item.Step_Name != null ? comObj.GetResxNameByValue_Oriya(item.Step_Name) : string.Empty;
                                item.Step_Description = item.Step_Description != null ? comObj.GetResxNameByValue_Oriya(item.Step_Description) : string.Empty;
                                if (item.CropSteps_Material.Count > 0)
                                {
                                    foreach (var itemMat in item.CropSteps_Material)
                                    {
                                        itemMat.Material_Name = itemMat.Material_Name != null ? comObj.GetResxNameByValue_Oriya(itemMat.Material_Name) : string.Empty;
                                        itemMat.Audio_Path = comObj.fetchAudioPahtMaterials(itemMat.Id, langId);
                                        itemMat.Image_Path = itemMat.Image_Path != null ? itemMat.Image_Path : string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty } , success = false, error = "There are no steps and Material for this Crop" });
                        }
                        break;
                    case "Santhali":
                        crop.CropName = crop.CropName != null ? comObj.GetResxNameByValue_Hindi(crop.CropName) : string.Empty;
                        crop.Title_Audio = comObj.fetchAudioPahtCrops(id, langId);
                        if (crop.Cultivation_Steps.Count > 0)
                        {
                            foreach (var item in crop.Cultivation_Steps)
                            {
                                item.Step_Name = item.Step_Name != null ? comObj.GetResxNameByValue_Hindi(item.Step_Name) : string.Empty;
                                item.Step_Description = item.Step_Description != null ? comObj.GetResxNameByValue_Hindi(item.Step_Description) : string.Empty;
                                item.Title_Audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Title");
                                item.Description_audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Description");
                                if (item.CropSteps_Material.Count > 0)
                                {
                                    foreach (var itemMat in item.CropSteps_Material)
                                    {
                                        itemMat.Material_Name = itemMat.Material_Name != null ? comObj.GetResxNameByValue_Hindi(itemMat.Material_Name) : string.Empty;
                                        itemMat.Audio_Path = comObj.fetchAudioPahtMaterials(itemMat.Id, langId);
                                        itemMat.Image_Path = itemMat.Image_Path != null ? itemMat.Image_Path : string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "There are no steps and Material for this Crop" });
                        }
                        break;
                    case "Ho":
                        crop.CropName = crop.CropName != null ? comObj.GetResxNameByValue_Hindi(crop.CropName) : string.Empty;
                        crop.Title_Audio = comObj.fetchAudioPahtCrops(id, langId);
                        if (crop.Cultivation_Steps.Count > 0)
                        {
                            foreach (var item in crop.Cultivation_Steps)
                            {
                                item.Step_Name = item.Step_Name != null ? comObj.GetResxNameByValue_Hindi(item.Step_Name) : string.Empty;
                                item.Step_Description = item.Step_Description != null ? comObj.GetResxNameByValue_Hindi(item.Step_Description) : string.Empty;
                                item.Title_Audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Title");
                                item.Description_audio = comObj.fetchAudioPahtSteps(item.Id, langId, "Description");
                                if (item.CropSteps_Material.Count > 0)
                                {
                                    foreach (var itemMat in item.CropSteps_Material)
                                    {
                                        itemMat.Material_Name = itemMat.Material_Name != null ? comObj.GetResxNameByValue_Hindi(itemMat.Material_Name) : string.Empty;
                                        itemMat.Audio_Path = comObj.fetchAudioPahtMaterials(itemMat.Id, langId);
                                        itemMat.Image_Path = itemMat.Image_Path != null ? itemMat.Image_Path : string.Empty;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "There are no steps and Material for this Crop" });
                        }
                        break;
                    default:
                        break;
                }

                if (crop == null)
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = "No Data" });
                }
                else
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = crop, success = true, error = (string)null });
                }
            }
            catch (Exception ex)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = (string)null, success = true, error = ex.Message });
            }
        }

        // PUT: api/Crops/5
        [HttpPost]
        public HttpResponseMessage PutCrop(int id, Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != crop.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var CropsData = db.Crops.Where(q => q.CropName.ToUpper() == crop.CropName.ToUpper()).Any() ? db.Crops.Where(p => p.CropName.ToUpper() == crop.CropName.ToUpper()).First() : null;
            if (CropsData != null && CropsData.Id != crop.Id)
            {
                if (db.Crops.Any(p => p.CropName.ToUpper() == crop.CropName.ToUpper()))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Crop Name already exists" });
                }
            }
            else
            {
                try
                {
                    Crop cropsData = db.Crops.Where(a => a.Id == id).FirstOrDefault();
                    cropsData.CropName = crop.CropName;
                    cropsData.Active = crop.Active;
                    cropsData.Ready = crop.Active;
                    cropsData.UpdatedBy = crop.UpdatedBy;
                    cropsData.UpdatedOn = crop.UpdatedOn;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CropExists(id))
                    {
                        return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { crop }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ActiveDeactiveCrop(int id, Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != crop.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //db.Entry(crop).State = EntityState.Modified;
            try
            {
                Crop cropsData = db.Crops.Where(a => a.Id == id).FirstOrDefault();
                cropsData.Active = crop.Active;
                cropsData.Ready = crop.Active;
                cropsData.ActiveBy = crop.ActiveBy;
                cropsData.ActiveOn = crop.ActiveOn;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { crop }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage UpdateCropImagePath(Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
           
            try
            {
                Crop cropsData = db.Crops.Where(a => a.Id == crop.Id).FirstOrDefault();
                cropsData.FilePath = crop.FilePath;
                
                if (crop.UpdatedBy != null)
                {
                    cropsData.UpdatedBy = crop.UpdatedBy;
                    cropsData.UpdatedOn = crop.UpdatedOn;
                }
                
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropExists(crop.Id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { crop }, success = true, error = string.Empty });
        }

        // POST: api/Crops
        [HttpPost]
        public HttpResponseMessage PostCrop(Crop crop)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            var DataFound = (from Cropata in db.Crops
                             where Cropata.CropName.ToUpper() == crop.CropName.ToUpper()
                             select Cropata.CropName).SingleOrDefault();

            if (DataFound == null)
            {
                db.Crops.Add(crop);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Crop Name already exists" });
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = crop.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Crops/5
        //[ResponseType(typeof(Crop))]
        //public IHttpActionResult DeleteCrop(int id)
        //{
        //    Crop crop = db.Crops.Find(id);
        //    if (crop == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Crops.Remove(crop);
        //    db.SaveChanges();

        //    return Ok(crop);
        //}

        [HttpGet]
        public IHttpActionResult DeativeCrop(int id)
        {
            Crop crop = db.Crops.Find(id);
            if (crop == null)
            {
                return NotFound();
            }

            db.Crops.Remove(crop);
            db.SaveChanges();

            return Ok(crop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropExists(int id)
        {
            return db.Crops.Count(e => e.Id == id) > 0;
        }
    }
}