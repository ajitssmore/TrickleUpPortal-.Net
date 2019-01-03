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
    public class MoneyManagersController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        ////GET: api/MoneyManagers
        //[HttpGet]
        //public IQueryable<MoneyManager> GetMoneyManagers()
        //{
        //    return db.MoneyManagers;
        //}

        [HttpGet]
        public HttpResponseMessage GetMoneyManagers()
        {
            //var transactions = from MoneyManager in db.MoneyManagers
            //                   select new { MoneyManager.Id, MoneyManager.UserId, MoneyManager.Mode, MoneyManager.Type, MoneyManager.Amount };
            var transactions = db.MoneyManagers;
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { transactions }, success = true, error = string.Empty });
        }

        public HttpResponseMessage GetMoneyManagersByUser(int userId)
        {
            var transactions = db.MoneyManagers.Where(a=>a.UserId == userId && a.Active == true);
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { transactions }, success = true, error = string.Empty });
        }

        // GET: api/MoneyManagers/5
        [ResponseType(typeof(MoneyManager))]
        public IHttpActionResult GetMoneyManager(int id)
        {
            MoneyManager moneyManager = db.MoneyManagers.Find(id);
            if (moneyManager == null)
            {
                return NotFound();
            }

            return Ok(moneyManager);
        }

        // PUT: api/MoneyManagers/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMoneyManager(int id, MoneyManager moneyManager)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != moneyManager.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(moneyManager).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MoneyManagerExists(id))
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
        public HttpResponseMessage PutMoneyManager(MoneyManager[] moneyManager)
        {
            foreach (var item in moneyManager)
            {
                //var moneyManagerdata = db.MoneyManagers.Where(a=>a.UID == item.UID).FirstOrDefault();
                //db.Entry(item).State = EntityState.Modified;
                //item.Id = moneyManagerdata.Id;
                //try
                //{
                //    db.SaveChanges();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    throw;
                //}

                //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/401abdff-0bdb-41a0-948d-6a9593255e6f/quotthe-property-id-is-part-of-the-objects-key-information-and-cannot-be-modified?forum=silverlightgen
                MoneyManager moneyManagerdata = db.MoneyManagers.Where(a => a.UID == item.UID).FirstOrDefault();
                if (item.UpdatedBy != null)
                {
                    moneyManagerdata.UpdatedBy = item.UpdatedBy;
                    moneyManagerdata.UpdatedOn = item.UpdatedOn;
                }
                if (item.ActiveBy != null)
                {
                    moneyManagerdata.ActiveBy = item.ActiveBy;
                    moneyManagerdata.ActiveOn = item.ActiveOn;
                }
                moneyManagerdata.Active = item.Active;
                db.SaveChanges();
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { moneyManager }, success = true, error = string.Empty });
        }


        // POST: api/MoneyManagers
        //[ResponseType(typeof(MoneyManager))]
        [HttpPost]
        public HttpResponseMessage PostMoneyManager(MoneyManager moneyManager)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            int UIDCount = db.MoneyManagers.Where(a => a.UID.ToString().ToLower() == moneyManager.UID.Value.ToString().ToLower()).Count();
            if (UIDCount > 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = "Money Manager UID already exists" });
            }

            db.MoneyManagers.Add(moneyManager);
            db.SaveChanges();

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = moneyManager.Id }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage PostBulkMoneyManager(MoneyManager[] moneyManager)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            foreach (var item in moneyManager)
            {
                db.MoneyManagers.Add(item);
                db.SaveChanges();
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { moneyManager }, success = true, error = string.Empty });
        }


        // DELETE: api/MoneyManagers/5
        [ResponseType(typeof(MoneyManager))]
        public IHttpActionResult DeleteMoneyManager(int id)
        {
            MoneyManager moneyManager = db.MoneyManagers.Find(id);
            if (moneyManager == null)
            {
                return NotFound();
            }

            db.MoneyManagers.Remove(moneyManager);
            db.SaveChanges();

            return Ok(moneyManager);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoneyManagerExists(int id)
        {
            return db.MoneyManagers.Count(e => e.Id == id) > 0;
        }
    }
}