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
using System.Text.RegularExpressions;
using TrickleUpPortal.Models;
using System.Web;
using System.IO;
using Microsoft.SqlServer.Server;
using System.Threading.Tasks;

namespace TrickleUpPortal.Controllers
{
    public class MediaContentsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        //// GET: api/MediaContents
        //public IQueryable<MediaContent> GetMediaContents()
        //{
        //    return db.MediaContents;
        //}

        [HttpGet]
        public IHttpActionResult GetMediaContents()
        {
            var result = from MediaContent in db.MediaContents
                         select new { MediaContent.Id, MediaContent.MediaType, MediaContent.MediaURL, MediaContent.FileName };
            return Ok(result);
        }

        //[HttpPost]
        //public IHttpActionResult UploadVideo(HttpPostedFileBase fileupload)
        //{
        //    if (fileupload != null)
        //    {
        //        string fileName = Path.GetFileName(fileupload.FileName);
        //        int fileSize = fileupload.ContentLength;
        //        int Size = fileSize / 1000;
        //        fileupload.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/MediaContent/" + fileName));

        //        //string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //        //using (SqlConnection con = new SqlConnection(CS))
        //        //{
        //        //    SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
        //        //    cmd.CommandType = CommandType.StoredProcedure;
        //        //    con.Open();
        //        //    cmd.Parameters.AddWithValue("@Name", fileName);
        //        //    cmd.Parameters.AddWithValue("@FileSize", Size);
        //        //    cmd.Parameters.AddWithValue("FilePath", "~/VideoFileUpload/" + fileName);
        //        //    cmd.ExecuteNonQuery();
        //        //}
        //    }
        //    return Ok("UploadVideo");
        //}

        [HttpPost]
        public IHttpActionResult UploadFile()
        {
            //https://www.c-sharpcorner.com/article/how-to-dynamically-upload-and-play-video-file-using-asp-net-mvc-5/
            //https://yogeshdotnet.com/web-api-2-file-upload-asp-net-mvc/
            List<string> savedFilePath = new List<string>();
            // Check if the request contains multipart/form-data
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //Get the path of folder where we want to upload all files.
            string rootPath = HttpContext.Current.Server.MapPath("~/MediaContent");
            var provider = new MultipartFileStreamProvider(rootPath);
            // Read the form data.
            //If any error(Cancelled or any fault) occurred during file read , return internal server error
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData dataitem in provider.FileData)
                    {
                        try
                        {
                            //Replace / from file name
                            string name = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");
                            //Create New file name using GUID to prevent duplicate file name
                            string newFileName = Guid.NewGuid() + Path.GetExtension(name);
                            //Move file from current location to target folder.
                            File.Move(dataitem.LocalFileName, Path.Combine(rootPath, newFileName));
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.Created, savedFilePath);
                });
            return Ok("UploadVideo");
        }


        // GET: api/MediaContents/5
        [ResponseType(typeof(MediaContent))]
        public IHttpActionResult GetMediaContent(int id)
        {
            MediaContent mediaContent = db.MediaContents.Find(id);
            if (mediaContent == null)
            {
                return NotFound();
            }

            return Ok(mediaContent);
        }

        // PUT: api/MediaContents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMediaContent(int id, MediaContent mediaContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mediaContent.Id)
            {
                return BadRequest();
            }

            db.Entry(mediaContent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaContentExists(id))
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

        // POST: api/MediaContents
        [ResponseType(typeof(MediaContent))]
        public IHttpActionResult PostMediaContent(MediaContent mediaContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MediaContents.Add(mediaContent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mediaContent.Id }, mediaContent);
        }

        // DELETE: api/MediaContents/5
        [ResponseType(typeof(MediaContent))]
        public IHttpActionResult DeleteMediaContent(int id)
        {
            MediaContent mediaContent = db.MediaContents.Find(id);
            if (mediaContent == null)
            {
                return NotFound();
            }

            db.MediaContents.Remove(mediaContent);
            db.SaveChanges();

            return Ok(mediaContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MediaContentExists(int id)
        {
            return db.MediaContents.Count(e => e.Id == id) > 0;
        }
    }
}