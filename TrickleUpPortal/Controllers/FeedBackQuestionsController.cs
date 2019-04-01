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
    public class FeedBackQuestionsController : ApiController
    {
        CommonController comObj = new CommonController();
        string LanguageName;
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/FeedBackQuestions
        //public IQueryable<FeedBackQuestion> GetFeedBackQuestions()
        //{
        //    return db.FeedBackQuestions;
        //}

        [HttpGet]
        public HttpResponseMessage GetFeedBackQuestions()
        {
            var question = from questions in db.FeedBackQuestions
                           select new { questions.Id, questions.Questions, questions.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { question }, success = true, error = string.Empty });
        }

        [HttpGet]
        public HttpResponseMessage GetFeedBackQuestionsBylang(int langCode)
        {
            LanguageName = comObj.fetchLang(langCode);

            List<FeedBackQuestion> question = new List<FeedBackQuestion>();
            var questiondata = (from questions in db.FeedBackQuestions
                           where questions.Active == true
                           select new { questions.Id, questions.Questions, questions.Active }).ToList();
            foreach (var item in questiondata)
            {
                FeedBackQuestion questionObj = new FeedBackQuestion();
                switch (LanguageName)
                {
                    case "Hindi":
                        questionObj.Id = item.Id;
                        questionObj.Questions = item.Questions != null ? comObj.GetResxNameByValue_Hindi(item.Questions) : string.Empty;
                        questionObj.Active = item.Active;
                        question.Add(questionObj);
                        break;
                    case "English":
                        questionObj.Id = item.Id;
                        questionObj.Questions = item.Questions;
                        questionObj.Active = item.Active;
                        question.Add(questionObj);
                        break;
                    case "Oriya":
                        questionObj.Id = item.Id;
                        questionObj.Questions = item.Questions != null ? comObj.GetResxNameByValue_Oriya(item.Questions) : string.Empty;
                        questionObj.Active = item.Active;
                        question.Add(questionObj);
                        break;
                    case "Santhali":
                        questionObj.Id = item.Id;
                        questionObj.Questions = item.Questions != null ? comObj.GetResxNameByValue_Hindi(item.Questions) : string.Empty;
                        questionObj.Active = item.Active;
                        question.Add(questionObj);
                        break;
                    case "Ho":
                        questionObj.Id = item.Id;
                        questionObj.Questions = item.Questions != null ? comObj.GetResxNameByValue_Hindi(item.Questions) : string.Empty;
                        questionObj.Active = item.Active;
                        question.Add(questionObj);
                        break;
                    default:
                        break;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { question }, success = true, error = string.Empty });
        }

        // GET: api/FeedBackQuestions/5
        [ResponseType(typeof(FeedBackQuestion))]
        public IHttpActionResult GetFeedBackQuestion(int id)
        {
            FeedBackQuestion feedBackQuestion = db.FeedBackQuestions.Find(id);
            if (feedBackQuestion == null)
            {
                return NotFound();
            }

            return Ok(feedBackQuestion);
        }

        // PUT: api/FeedBackQuestions/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutFeedBackQuestion(int id, FeedBackQuestion feedBackQuestion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != feedBackQuestion.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(feedBackQuestion).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FeedBackQuestionExists(id))
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
        public HttpResponseMessage PutFeedBackQuestion(int id, FeedBackQuestion feedBackQuestion)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != feedBackQuestion.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            //db.Entry(feedBackQuestion).State = EntityState.Modified;

            try
            {
                FeedBackQuestion feedBackQuestiondata = db.FeedBackQuestions.Where(a => a.Id == feedBackQuestion.Id).FirstOrDefault();
                feedBackQuestiondata.Questions = feedBackQuestion.Questions;
                feedBackQuestiondata.Active = feedBackQuestion.Active;
                
                if (feedBackQuestion.UpdatedBy != null)
                {
                    feedBackQuestiondata.UpdatedBy = feedBackQuestion.UpdatedBy;
                    feedBackQuestiondata.UpdatedOn = feedBackQuestion.UpdatedOn;
                }
                if (feedBackQuestion.ActiveBy != null)
                {
                    feedBackQuestiondata.ActiveBy = feedBackQuestion.ActiveBy;
                    feedBackQuestiondata.ActiveOn = feedBackQuestion.ActiveOn;
                }
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedBackQuestionExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { feedBackQuestion }, success = true, error = string.Empty });
        }


        // POST: api/FeedBackQuestions
        //[ResponseType(typeof(FeedBackQuestion))]
        //public IHttpActionResult PostFeedBackQuestion(FeedBackQuestion feedBackQuestion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.FeedBackQuestions.Add(feedBackQuestion);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = feedBackQuestion.Id }, feedBackQuestion);
        //}

        [HttpPost]
        public HttpResponseMessage PostFeedBackQuestion(FeedBackQuestion feedBackQuestion)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.FeedBackQuestions.Add(feedBackQuestion);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = feedBackQuestion.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/FeedBackQuestions/5
        [ResponseType(typeof(FeedBackQuestion))]
        public IHttpActionResult DeleteFeedBackQuestion(int id)
        {
            FeedBackQuestion feedBackQuestion = db.FeedBackQuestions.Find(id);
            if (feedBackQuestion == null)
            {
                return NotFound();
            }

            db.FeedBackQuestions.Remove(feedBackQuestion);
            db.SaveChanges();

            return Ok(feedBackQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedBackQuestionExists(int id)
        {
            return db.FeedBackQuestions.Count(e => e.Id == id) > 0;
        }
    }
}