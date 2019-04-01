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
using System.Web.Script.Serialization;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class UserFeedbacksController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        CommonController comObj = new CommonController();
        string LanguageName = "", ErrorMessage = "";

        //GET: api/UserFeedbacks
        //public IQueryable<UserFeedback> GetUserFeedbacks()
        //{
        //    return db.UserFeedbacks;
        //}

        [HttpGet]
        public HttpResponseMessage GetUserFeedbacks()
        {
            var UserFeedbacks = from UserFeedback in db.UserFeedbacks
                            join User in db.Users on UserFeedback.UserId equals User.Id into UserNew
                                from User in UserNew.DefaultIfEmpty()
                            select new { UserFeedback.Id, UserFeedback.FeedBack, User.Name, UserFeedback.CreatedOn };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { UserFeedbacks }, success = true, error = string.Empty });
        }

        //[HttpGet]
        //public HttpResponseMessage GetUserFeedbacks()
        //{
        //    var UserFeedbacks = db.Users.Join(
        //    db.UserFeedbacks.GroupBy(r => r.UserId).Select(g => new { UserId = g.Key, Feeback = g.Average(r => r.FeedBack) }),
        //    m => m.Id,
        //    r => r.UserId,
        //    (m, r) => new { userId = m.Id,  Name = m.Name, Feedback = r.Feeback }).ToList();

        //    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { UserFeedbacks }, success = true, error = string.Empty });
        //}

        // GET: api/UserFeedbacks/5
        [ResponseType(typeof(UserFeedback))]
        public IHttpActionResult GetUserFeedback(int id)
        {
            UserFeedback userFeedback = db.UserFeedbacks.Find(id);
            if (userFeedback == null)
            {
                return NotFound();
            }

            return Ok(userFeedback);
        }

        // PUT: api/UserFeedbacks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserFeedback(int id, UserFeedback userFeedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userFeedback.Id)
            {
                return BadRequest();
            }

            db.Entry(userFeedback).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFeedbackExists(id))
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

        // POST: api/UserFeedbacks
        //[ResponseType(typeof(UserFeedback))]
        //public IHttpActionResult PostUserFeedback(UserFeedback userFeedback)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.UserFeedbacks.Add(userFeedback);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = userFeedback.Id }, userFeedback);
        //}

        public HttpResponseMessage PostUserFeedback(UserFeedback[] userFeedback, int languageId)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            LanguageName = comObj.fetchLang(languageId);

            foreach (var item in userFeedback)
            {
                string feedTime = string.Empty;
                string sMonth = DateTime.Now.ToString("MM");
                //List<string> feeddatetime = db.UserFeedbacks.Where(a => a.UserId == item.UserId).OrderByDescending(s => s.CreatedOn).Take(1).Select(x => new { x.CreatedOn }).ToList();
                List<UserFeedback> feedbackdata = db.UserFeedbacks.Where(a => a.UserId == item.UserId).OrderByDescending(s => s.CreatedOn).Take(1).ToList();
                if (feedbackdata.Count == 1)
                {
                    foreach (var feedData in feedbackdata)
                    {
                        if (feedData.CreatedOn != null)
                        {
                            DateTime feeddatetime = (DateTime)feedData.CreatedOn;
                            feedTime = feeddatetime.ToString("MM");
                        }
                        else
                        {
                            feedTime = "";
                        }
                    }
                }

                switch (LanguageName)
                {
                    case "Hindi":
                        ErrorMessage = comObj.GetResxNameByValue_Hindi("Your feedback is already submitted");
                        break;
                    case "English":
                        ErrorMessage = "Your feedback is already submitted";
                        break;
                    case "Oriya":
                        ErrorMessage = comObj.GetResxNameByValue_Oriya("Your feedback is already submitted");
                        break;
                    case "Santhali":
                        ErrorMessage = comObj.GetResxNameByValue_Hindi("Your feedback is already submitted");
                        break;
                    case "Ho":
                        ErrorMessage = comObj.GetResxNameByValue_Hindi("Your feedback is already submitted");
                        break;
                    default:
                        ErrorMessage = "The UserName or Password is Incorrect.";
                        break;
                }

                if (sMonth == feedTime)
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = ErrorMessage });
                }

                db.UserFeedbacks.Add(item);
                db.SaveChanges();
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userFeedback }, success = true, error = string.Empty });
        }

        // DELETE: api/UserFeedbacks/5
        [ResponseType(typeof(UserFeedback))]
        public IHttpActionResult DeleteUserFeedback(int id)
        {
            UserFeedback userFeedback = db.UserFeedbacks.Find(id);
            if (userFeedback == null)
            {
                return NotFound();
            }

            db.UserFeedbacks.Remove(userFeedback);
            db.SaveChanges();

            return Ok(userFeedback);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserFeedbackExists(int id)
        {
            return db.UserFeedbacks.Count(e => e.Id == id) > 0;
        }
    }
}