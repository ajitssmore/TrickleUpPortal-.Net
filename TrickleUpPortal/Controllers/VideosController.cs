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
    public class VideosController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Videos
        //public IQueryable<Video> GetVideos()
        //{
        //    return db.Videos;
        //}

        public HttpResponseMessage GetVideos()
        {
            var Videos = from Video in db.Videos
                         select new { Video.Id, Video.VideoName, Video.FilePath, Video.FileSize, Video.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Videos }, success = true, error = string.Empty });
        }

        // GET: api/Videos/5
        [ResponseType(typeof(Video))]
        public IHttpActionResult GetVideo(int id)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            return Ok(video);
        }

        // PUT: api/Videos/5
        //[HttpPost]
        //public IHttpActionResult PutVideo(int id, Video video)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != video.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(video).State = EntityState.Modified;

        //    try
        //    {
        //        //Video videodata = db.Videos.Where(a => a.Id == video.Id).FirstOrDefault();
        //        //videodata.Active = video.Active;
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!VideoExists(id))
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
        public HttpResponseMessage DeactiveVideo(int id, Video video)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != video.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (video.Active == false)
            {
                var CropVideoData = db.CropStep_VideoAllocation.Where(a => a.VideoId == video.Id).Any();
                //var StepVideoData = db.Cultivation_Steps.Where(q => q.VideoPath.ToUpper() == video.FilePath.ToUpper()).Any();
                var StepVideoData = db.CropStep_VideoAllocation.Where(a => a.VideoId == video.Id).Any();
                var MaterialVideoData = db.CropStepMaterial_VideoAllocation.Where(a => a.VideoId == video.Id).Any();
                if (CropVideoData == true || StepVideoData == true || MaterialVideoData==true)
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "You can't delete the file, because file has been allocated." });
                }
            }
            
            
            try
            {
                Video videodata = db.Videos.Where(a => a.Id == video.Id).FirstOrDefault();
                videodata.Active = video.Active;
                videodata.ActiveBy = video.ActiveBy;
                videodata.ActiveOn = video.ActiveOn;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }
            

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { video }, success = true, error = string.Empty });
        }
        // POST: api/Videos
        [ResponseType(typeof(Video))]
        public HttpResponseMessage PostVideo(Video video)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Videos.Add(video);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = video.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Videos/5
        [ResponseType(typeof(Video))]
        public IHttpActionResult DeleteVideo(int id)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }

            db.Videos.Remove(video);
            db.SaveChanges();

            return Ok(video);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VideoExists(int id)
        {
            return db.Videos.Count(e => e.Id == id) > 0;
        }
    }
}