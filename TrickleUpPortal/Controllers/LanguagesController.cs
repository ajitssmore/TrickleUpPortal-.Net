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
    public class LanguagesController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Languages
        //public IQueryable<Language> GetLanguages()
        //{
        //    return db.Languages;
        //}

        public HttpResponseMessage GetLanguages()
        {
            var Languages = from Language in db.Languages
                         select new { Language.Id, Language.LanguageCode, Language.LanguageName, Language.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { Languages }, success = true, error = string.Empty });
        }

        // GET: api/Languages/5
        [ResponseType(typeof(Language))]
        public IHttpActionResult GetLanguage(int id)
        {
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return NotFound();
            }

            return Ok(language);
        }

        // PUT: api/Languages/5
        [HttpPost]
        public HttpResponseMessage PutLanguage(int id, Language language)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != language.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(language).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { language }, success = true, error = string.Empty });
        }

        // POST: api/Languages
        [HttpPost]
        public HttpResponseMessage PostLanguage(Language language)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Languages.Add(language);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = language.Id }, success = true, error = string.Empty });
        }

        // DELETE: api/Languages/5
        //[ResponseType(typeof(Language))]
        //public IHttpActionResult DeleteLanguage(int id)
        //{
        //    Language language = db.Languages.Find(id);
        //    if (language == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Languages.Remove(language);
        //    db.SaveChanges();

        //    return Ok(language);
        //}

        [HttpGet]
        public IHttpActionResult DeativeLanguage(int id)
        {
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return NotFound();
            }

            db.Languages.Remove(language);
            db.SaveChanges();

            return Ok(language);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageExists(int id)
        {
            return db.Languages.Count(e => e.Id == id) > 0;
        }
    }
}