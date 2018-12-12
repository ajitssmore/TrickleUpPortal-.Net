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
using System.Web.Http.Cors;
using System.Configuration;

namespace TrickleUpPortal.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatesController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        CommonController comObj = new CommonController();
        // GET: api/States
        //public IQueryable<State> GetStates()
        //{
        //    return db.States;
        //}

        [HttpGet]
        public HttpResponseMessage GetStates()
        {
            var States = from State in db.States
                         select new { State.Id, State.StateName, State.StateCode, State.Active };
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { States }, success = true, error = string.Empty });
        }

        // GET: api/States/5
        [ResponseType(typeof(State))]
        public IHttpActionResult GetState(int id)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        // PUT: api/States/5
        //[ResponseType(typeof(void))]
        [HttpPost]
        public HttpResponseMessage PutState(int id, State state)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != state.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(state).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    //return NotFound();
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { state }, success = true, error = string.Empty });
        }

        // POST: api/States
        [ResponseType(typeof(State))]
        public HttpResponseMessage PostState(State state)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            try
            {
                var DataFound = (from statedata in db.States
                       where statedata.StateName.ToUpper() == state.StateName.ToUpper() select statedata.StateName).SingleOrDefault();

            if (DataFound == null)
            {
                    db.States.Add(state);
                    db.SaveChanges();
            }
            else
            {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "State Name already exits" });
            }

                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = state.Id }, success = true, error = string.Empty });
            }
            catch (Exception ex)
            {
                long ExceptionId = comObj.SendExcepToDB(ex);
                Tbl_ExceptionLogging tbl_ExceptionLogging = db.Tbl_ExceptionLogging.Find(ExceptionId);
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { tbl_ExceptionLogging }, success = false, error = string.Empty });
                //ExceptionLogging.SendExcepToDB(ex);
                //Label1.Text = "Some Technical Error occurred,Please visit after some time";

            }

        }

        // DELETE: api/States/5
        [ResponseType(typeof(State))]
        public IHttpActionResult DeleteState(int id)
        {
            State state = db.States.Find(id);
            if (state == null)
            {
                return NotFound();
            }

            db.States.Remove(state);
            db.SaveChanges();

            return Ok(state);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StateExists(int id)
        {
            return db.States.Count(e => e.Id == id) > 0;
        }
    }
}