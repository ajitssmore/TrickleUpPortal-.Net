using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class UserCredentialsController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();
        CommonController comObj = new CommonController();
        string LanguageName;

        // GET: api/UserCredentials
        public IQueryable<UserCredential> GetUserCredentials()
        {
            return db.UserCredentials;
        }

        // GET: api/UserCredentials/5
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult GetUserCredential(int id)
        {
            UserCredential userCredential = db.UserCredentials.Where(a=>a.UserId == id).FirstOrDefault();
            if (userCredential == null)
            {
                return NotFound();
            }

            return Ok(userCredential);
        }

        // GET: api/UserCredentials/AuthenticateUser/
        [HttpPost]
        public HttpResponseMessage AuthenticateUser(UserCredential userCredential)
        {
            try
            {
                LanguageName = comObj.fetchLang(userCredential.LangCode);

                var result = from UserCredential in db.UserCredentials
                             join User in db.Users on UserCredential.UserId equals User.Id
                             join Role in db.Roles on User.Role equals Role.Id
                             where ((UserCredential.UserName == userCredential.UserName || UserCredential.PhoneNumber == userCredential.UserName) && UserCredential.Password == userCredential.Password && User.Active == true)
                             select new { UserCredential.UserId, UserCredential.UserName, User.PhoneNumber, UserCredential.Id, User.Name, User.Role, Role.RoleName, User.ImagePath, User.Active };
                

                dynamic UserVerification = new ExpandoObject();
                //List<Object> dataList = new List<Object>();
                var Isauthenticated = false;
                foreach (var item in result)
                {
                    Isauthenticated = true;
                    UserVerification.UserId = item.UserId;
                    UserVerification.Id = item.Id;
                    UserVerification.UserName = item.UserName;
                    UserVerification.PhoneNumber = item.PhoneNumber;
                    UserVerification.authenticated = true;
                    UserVerification.Name = item.Name;
                    UserVerification.role = item.Role;
                    UserVerification.image = item.ImagePath;
                    UserVerification.RoleName = item.RoleName;
                }
                if (Isauthenticated == false)
                {
                    string errorMessage = string.Empty;
                    switch (LanguageName)
                    {
                        case "Hindi":
                            errorMessage = "यूजरनेम या पासवर्ड गलत है।";
                            break;
                        case "English":
                            errorMessage = "The UserName or Password is Incorrect.";
                            break;
                        case "Oriya":
                            errorMessage = "ଥେ ଉସେର୍ଣ୍ଣାମେ ଡ ପାସ୍ରେବୋର୍ଡ ଇସଃ ଇଂକରରସତ.";
                            break;
                        default:
                            errorMessage = "The UserName or Password is Incorrect.";
                            break;
                    }
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = errorMessage });
                }
                else
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = UserVerification, success = true, error = (string)null });
                }
                
            }
            catch (Exception ex)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = (string)null, success = true, error = ex.Message });
            }

        }

        [HttpPost]
        public HttpResponseMessage ChangePassword(UserPasswordModel passwordData)
        {
            var result = from UserCredential in db.UserCredentials
                         where ((UserCredential.UserId == passwordData.UserId) && (UserCredential.Password == passwordData.OldPassword))
                         select new { UserCredential.Id, UserCredential.UserName};
            if (result.Count() <= 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = "Old Password is incorrect" });
            }
            else
            {
                    db.UserCredentials.Where(x => x.UserId == passwordData.UserId).ToList().ForEach(x =>
                    {
                        x.Password = passwordData.NewPassword;
                    });
                    db.SaveChanges();
                
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { passwordData }, success = true, error = string.Empty });
            }
        }

        // PUT: api/UserCredentials/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserCredential(int id, UserCredential userCredential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userCredential.Id)
            {
                return BadRequest();
            }

            db.Entry(userCredential).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCredentialExists(id))
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

        // POST: api/UserCredentials
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult PostUserCredential(UserCredential userCredential)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserCredentials.Add(userCredential);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserCredentialExists(userCredential.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userCredential.Id }, userCredential);
        }

        // DELETE: api/UserCredentials/5
        [ResponseType(typeof(UserCredential))]
        public IHttpActionResult DeleteUserCredential(int id)
        {
            UserCredential userCredential = db.UserCredentials.Find(id);
            if (userCredential == null)
            {
                return NotFound();
            }

            db.UserCredentials.Remove(userCredential);
            db.SaveChanges();

            return Ok(userCredential);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserCredentialExists(int id)
        {
            return db.UserCredentials.Count(e => e.Id == id) > 0;
        }
    }
}