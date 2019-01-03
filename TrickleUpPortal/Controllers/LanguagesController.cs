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
                            orderby Language.LanguageCode ascending
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
            var languageData = db.Languages.Where(q => q.LanguageName.ToUpper() == language.LanguageName.ToUpper()).Any() ? db.Languages.Where(p => p.LanguageName.ToUpper() == language.LanguageName.ToUpper()).First() : null;
            if (languageData != null && languageData.Id != language.Id)
            {
                if (db.Languages.Any(p => p.LanguageName.ToUpper() == language.LanguageName.ToUpper()))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Language Name already exists" });
                }
            }
            else
            {
                try
                {
                    Language LanguageUpdateData = db.Languages.Where(a => a.Id == language.Id).FirstOrDefault();
                    LanguageUpdateData.LanguageCode= language.LanguageCode;
                    LanguageUpdateData.LanguageName = language.LanguageName;
                    LanguageUpdateData.UpdatedBy = language.UpdatedBy;
                    LanguageUpdateData.UpdatedOn = language.UpdatedOn;
                    LanguageUpdateData.Active = language.Active;
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
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { language }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ActiveDeactiveLanguage(int id, Language language)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != language.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            try
            {
                Language LanguageUpdateData = db.Languages.Where(a => a.Id == language.Id).FirstOrDefault();
                LanguageUpdateData.ActiveBy = language.ActiveBy;
                LanguageUpdateData.ActiveOn = language.ActiveOn;
                LanguageUpdateData.Active = language.Active;
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

            var DataFound = (from Languagedata in db.Languages
                             where Languagedata.LanguageName.ToUpper() == language.LanguageName.ToUpper()
                             select Languagedata.LanguageName).SingleOrDefault();

            if (DataFound == null)
            {
                db.Languages.Add(language);
                db.SaveChanges();
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Language Name already exists" });
            }
            
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