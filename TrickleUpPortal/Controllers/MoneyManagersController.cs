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

        // GET: api/MoneyManagers
        //public IQueryable<MoneyManager> GetMoneyManagers()
        //{
        //    return db.MoneyManagers;
        //}

        [HttpGet]
        public HttpResponseMessage GetMoneyManagers()
        {
            var transactions = from MoneyManager in db.MoneyManagers
                         select new { MoneyManager.Id, MoneyManager.UserId, MoneyManager.Mode, MoneyManager.Type, MoneyManager.Amount };
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMoneyManager(int id, MoneyManager moneyManager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != moneyManager.Id)
            {
                return BadRequest();
            }

            db.Entry(moneyManager).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoneyManagerExists(id))
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

        // POST: api/MoneyManagers
        //[ResponseType(typeof(MoneyManager))]
        [HttpPost]
        public HttpResponseMessage PostMoneyManager(MoneyManager moneyManager)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
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