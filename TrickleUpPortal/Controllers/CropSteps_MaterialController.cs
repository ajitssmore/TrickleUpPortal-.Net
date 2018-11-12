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
    public class CropSteps_MaterialController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        //string LanguageName;
        CommonController comObj = new CommonController();
        // GET: api/CropSteps_Material
        //public IQueryable<CropSteps_Material> GetCropSteps_Material()
        //{
        //    return db.CropSteps_Material;
        //}

        [HttpGet]
        public HttpResponseMessage GetCropSteps_Material()
        {
            var CropSteps_Material = from CropSteps_Materials in db.CropSteps_Material
                                    join Cultivation_Step in db.Cultivation_Steps on CropSteps_Materials.Step_Id equals Cultivation_Step.Id
                                    select new { CropSteps_Materials.Id, CropSteps_Materials.Step_Id, CropSteps_Materials.Material_Name, CropSteps_Materials.Material_Transaction, CropSteps_Materials.Per_Decimal_Price, CropSteps_Materials.Quantity, Cultivation_Step.Step_Name };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { CropSteps_Material }, success = true, error = string.Empty });
        }
        

        // GET: api/CropSteps_Material/5
        [ResponseType(typeof(CropSteps_Material))]
        public IHttpActionResult GetCropSteps_Material(int id)
        {
            CropSteps_Material cropSteps_Material = db.CropSteps_Material.Find(id);
            if (cropSteps_Material == null)
            {
                return NotFound();
            }

            return Ok(cropSteps_Material);
        }

        // PUT: api/CropSteps_Material/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCropSteps_Material(int id, CropSteps_Material cropSteps_Material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cropSteps_Material.Id)
            {
                return BadRequest();
            }

            db.Entry(cropSteps_Material).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropSteps_MaterialExists(id))
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

        // POST: api/CropSteps_Material
        [ResponseType(typeof(CropSteps_Material))]
        public IHttpActionResult PostCropSteps_Material(CropSteps_Material cropSteps_Material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CropSteps_Material.Add(cropSteps_Material);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cropSteps_Material.Id }, cropSteps_Material);
        }

        // DELETE: api/CropSteps_Material/5
        [ResponseType(typeof(CropSteps_Material))]
        public IHttpActionResult DeleteCropSteps_Material(int id)
        {
            CropSteps_Material cropSteps_Material = db.CropSteps_Material.Find(id);
            if (cropSteps_Material == null)
            {
                return NotFound();
            }

            db.CropSteps_Material.Remove(cropSteps_Material);
            db.SaveChanges();

            return Ok(cropSteps_Material);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CropSteps_MaterialExists(int id)
        {
            return db.CropSteps_Material.Count(e => e.Id == id) > 0;
        }
    }
}