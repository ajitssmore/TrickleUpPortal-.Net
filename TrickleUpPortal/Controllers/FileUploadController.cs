using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class FileUploadController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        //// GET: api/Crops
        public string GetCrops()
        {
            return "hello";
        }

        [HttpPost]
        public HttpResponseMessage PostImages()
        {
            var docfiles = new List<string>();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/MediaContent/Images/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    Image Imagedata = new Image();
                    int index = postedFile.FileName.IndexOf('.');
                    Imagedata.ImageName = postedFile.FileName.Substring(0, index);
                    decimal size = Math.Round(((decimal)postedFile.ContentLength / (decimal)1024), 2);
                    Imagedata.FileSize = Convert.ToString(size) + " kb";
                    Imagedata.FilePath = @"MediaContent\Images\" + postedFile.FileName + "";
                    Imagedata.CreatedBy = 1;
                    Imagedata.CreatedOn = System.DateTime.Now;
                    Imagedata.Active = true;
                    PostImage(Imagedata);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { docfiles }, success = true, error = string.Empty });
        }
        [HttpPost]
        public HttpResponseMessage PostVideos()
        {
            var docfiles = new List<string>();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/MediaContent/Videos/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    VideosController VideoObj = new VideosController();
                    Video Videodata = new Video();
                    int index = postedFile.FileName.IndexOf('.');
                    Videodata.VideoName = postedFile.FileName.Substring(0, index);
                    decimal size = Math.Round(((decimal)postedFile.ContentLength / (decimal)1024), 2);
                    Videodata.FileSize = Convert.ToString(size) + " kb";
                    Videodata.FilePath = @"MediaContent\Videos\" + postedFile.FileName + "";
                    Videodata.CreatedBy = 1;
                    Videodata.CreatedOn = System.DateTime.Now;
                    Videodata.Active = true;
                    PostVideo(Videodata);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { docfiles }, success = true, error = string.Empty });
        }
        [HttpPost]
        public HttpResponseMessage PostAudios()
        {
            var docfiles = new List<string>();
            HttpResponseMessage result = null; 
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/MediaContent/Audios/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    AudiosController audioObj = new AudiosController();
                    Audio Audiodata = new Audio();
                    int index = postedFile.FileName.IndexOf('.');
                    Audiodata.FileName = postedFile.FileName.Substring(0, index);
                    decimal size = Math.Round(((decimal)postedFile.ContentLength / (decimal)1024), 2);
                    Audiodata.FileSize = Convert.ToString(size) + " kb";
                    Audiodata.FilePath = @"MediaContent\Audios\"+ postedFile.FileName + "";
                    Audiodata.Active = true;
                    PostAudio(Audiodata);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
                //result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { docfiles }, success = true, error = string.Empty });
        }

        public void PostAudio(Audio audio)
        {
            db.Audios.Add(audio);
            db.SaveChanges();
        }

        public void PostVideo(Video video)
        {
            db.Videos.Add(video);
            db.SaveChanges();
        }
        public void PostImage(Image image)
        {
            db.Images.Add(image);
            db.SaveChanges();
        }
        [HttpPost]
        public HttpResponseMessage PostUserImages()
        {
            //http://www.developerslearnit.com/2018/02/upload-file-with-other-body-parameters-in-aspnet-web-api.html
            var docfiles = new List<string>();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    Guid ImageId = Guid.NewGuid();
                    var fileName = String.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(postedFile.FileName), ImageId, Path.GetExtension(postedFile.FileName));
                    var ImagepathId = Path.Combine(Path.GetDirectoryName(postedFile.FileName), fileName);

                    var filePath = HttpContext.Current.Server.MapPath("~/MediaContent/UserImages/" + ImagepathId);
                    int UserId = int.Parse(HttpContext.Current.Request.Params.Get("UserId"));
                    string oldImagePath = HttpContext.Current.Request.Params.Get("OldfilePath");
                    postedFile.SaveAs(filePath);
                    //Image Imagedata = new Image();
                    //int index = postedFile.FileName.IndexOf('.');
                    //Imagedata.ImageName = postedFile.FileName.Substring(0, index);
                    //decimal size = Math.Round(((decimal)postedFile.ContentLength / (decimal)1024), 2);
                    //Imagedata.FileSize = Convert.ToString(size) + " kb";
                    //Imagedata.FilePath = @"MediaContent\UserImages\" + postedFile.FileName + "_" + ImageId;
                    //Imagedata.CreatedBy = 1;
                    //Imagedata.CreatedOn = System.DateTime.Now;
                    //Imagedata.Active = true;
                    string FilePath = @"MediaContent\UserImages\" + ImagepathId;
                    UpdateUserImage(UserId, FilePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { docfiles }, success = true, error = string.Empty });
        }

        public void UpdateUserImage(int id, string UserImagePath)
        {
                User UserUpdateData = db.Users.Where(a => a.Id == id).FirstOrDefault();
                UserUpdateData.ImagePath = UserImagePath;
                db.SaveChanges();
        }
    }
}
