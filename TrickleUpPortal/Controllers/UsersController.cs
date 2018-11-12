using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class UsersController : ApiController
    {
        public string bulkUploadId; public int UserCreateBy; public DateTime UserCreatedOn;
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Users
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        // GET: api/Users
        public HttpResponseMessage GetAllUsers()
        {
            var result = from User in db.Users
                         join Gender in db.Genders on User.Gender equals Gender.Id into GenderNew
                         from Gender in GenderNew.DefaultIfEmpty()
                         join State in db.States on User.State equals State.Id into StateNew
                         from State in StateNew.DefaultIfEmpty()
                         join Role in db.Roles on User.Role equals Role.Id into RoleNew
                         from Role in RoleNew.DefaultIfEmpty()
                         join Lang in db.Languages on User.Language equals Lang.Id into LangNew
                         from Lang in LangNew.DefaultIfEmpty()
                         join District in db.Districts on User.District equals District.Id into DistrictNew
                         from District in DistrictNew.DefaultIfEmpty()
                         join Village in db.Villages on User.Village equals Village.Id into VillageNew
                         from Village in VillageNew.DefaultIfEmpty()
                         join Grampanchayat in db.Grampanchayats on User.Grampanchayat equals Grampanchayat.Id into GrampanchayatNew
                         from Grampanchayat in GrampanchayatNew.DefaultIfEmpty()
                         select new
                         {
                             User.Id,
                             User.UserId,
                             User.Name,
                             User.PhoneNumber,
                             User.Age,
                             User.Gender,
                             Gender.GenderName,
                             User.State,
                             State.StateName,
                             User.District,
                             District.DistrictName,
                             User.Village,
                             Village.VillageName,
                             User.Grampanchayat,
                             Grampanchayat.GrampanchayatName,
                             User.Aadhaar,
                             User.Role,
                             Role.RoleName,
                             User.Language,
                             Lang.LanguageName,
                             User.IMEI1,
                             User.IMEI2,
                             User.FCMToken,
                             User.Active,
                             User.ImagePath
                         };
                         //select new { User.Id, User.UserId, User.Name, User.PhoneNumber, User.Age, User.Gender, Gender.GenderName, User.State, State.StateName, User.District, District.DistrictName, User.Village, Village.VillageName, User.Grampanchayat, Grampanchayat.GrampanchayatName, User.Aadhaar, User.Role, Role.RoleName, User.Language, Lang.LanguageName, User.IMEI1, User.IMEI2, User.FCMToken, User.Active, User.ImagePath };


            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = result, success = true, error = string.Empty });
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public HttpResponseMessage GetUser(int id)
        {
            //User user = db.Users.Find(id);
            //if (user == null)
            //{
            //    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = string.Empty, success = false, error = string.Empty });
            //}

            var Users = from User in db.Users
                        join Gender in db.Genders on User.Gender equals Gender.Id into GenderNew
                        from Gender in GenderNew.DefaultIfEmpty()
                        join State in db.States on User.State equals State.Id into StateNew
                        from State in StateNew.DefaultIfEmpty()
                        join Role in db.Roles on User.Role equals Role.Id into RoleNew
                        from Role in RoleNew.DefaultIfEmpty()
                        join Lang in db.Languages on User.Language equals Lang.Id into LangNew
                        from Lang in LangNew.DefaultIfEmpty()
                        join District in db.Districts on User.District equals District.Id into DistrictNew
                        from District in DistrictNew.DefaultIfEmpty()
                        join Village in db.Villages on User.Village equals Village.Id into VillageNew
                        from Village in VillageNew.DefaultIfEmpty()
                        join Grampanchayat in db.Grampanchayats on User.Grampanchayat equals Grampanchayat.Id into GrampanchayatNew
                        from Grampanchayat in GrampanchayatNew.DefaultIfEmpty()
                        where User.Id == id
                        select new { User.Id, User.UserId, User.Name, User.PhoneNumber, User.Age, User.Gender, Gender.GenderName, User.State, State.StateName, User.District, District.DistrictName, User.Village, Village.VillageName,  User.Grampanchayat,Grampanchayat.GrampanchayatName,  User.Aadhaar, User.Role, Role.RoleName, User.Language, Lang.LanguageName, User.IMEI1, User.IMEI2, User.FCMToken, User.Active, User.ImagePath };

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new {data = new { Users } , success = true, error = string.Empty });
        }

        // PUT: api/Users/5
        [HttpPost]
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != user.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.NotFound, new { data = new { string.Empty }, success = false, error = string.Empty });
                }
                else
                {
                    throw;
                }
            }

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { user }, success = true, error = string.Empty });
        }

        // POST: api/Users


        [HttpPost]
        public HttpResponseMessage PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (user.PhoneNumber != null)
            {
                int PhoneCount = db.Users.Where(a => a.PhoneNumber == user.PhoneNumber).ToList().Count;
                if (PhoneCount > 0)
                { 
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = "Phone Number already Exist" });
                }
            }

            if (user.UserId != null)
            {
                int UserIdCount = db.Users.Where(a => a.UserId == user.UserId).ToList().Count();
                if (UserIdCount > 0)
                { 
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = "User Name already Exist" });
                }
            }
            
            db.Users.Add(user);
            db.SaveChanges();
            CreateUserCrednetails(user);
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { id = user.Id }, success = true, error = string.Empty });
        }

        [HttpPost]
        public HttpResponseMessage ValidateUploadUser(BulkUserDataModel[] userList)
        {
            StringBuilder ErrorMerssage = new StringBuilder();
            bool ErrorMesFlag = false;
            foreach (var userdetails in userList)
            {
                ErrorMerssage.Length = 0;
                User Usersdata = new User();
                if (userdetails.PhoneNumber != null)
                {
                    int PhoneCount = db.Users.Where(a => a.PhoneNumber == userdetails.PhoneNumber).ToList().Count;
                    if (PhoneCount > 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append("Phone Number already Exits");
                    }
                }

                if (userdetails.UserName != null)
                {
                    int UserIdCount = db.Users.Where(a => a.UserId == userdetails.UserName).ToList().Count();
                    if (UserIdCount > 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", User Name already Exits");
                    }
                }
                userdetails.ErrorMessage = ErrorMerssage.ToString();
            }

            if (ErrorMesFlag == true)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userList }, success = false, error = string.Empty });
            }
            else
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userList }, success = true, error = string.Empty });
            }
            
        }

        [HttpPost]
        public HttpResponseMessage BulkUploadUser(BulkUserDataModel[] userList)
        {
            List<User> UsersList = new List<User>();
            foreach (var userdetails in userList)
            {
                User Usersdata = new User();
                Usersdata.Active = Convert.ToBoolean(userdetails.Active);
                Usersdata.Name = userdetails.Name;
                Usersdata.PhoneNumber = userdetails.PhoneNumber;
                Usersdata.Age = Convert.ToInt32(userdetails.Age);
                Usersdata.Gender = userdetails.Gender != null ? db.Genders.Where(a => a.GenderName == userdetails.Gender).FirstOrDefault().Id: 0;
                Usersdata.State = userdetails.State != null ? db.States.Where(a => a.StateName == userdetails.State).FirstOrDefault().Id : 0;
                Usersdata.District = userdetails.District != null ? db.Districts.Where(a => a.DistrictName == userdetails.District).FirstOrDefault().Id : 0;
                Usersdata.Village = userdetails.Village != null ? db.Villages.Where(a => a.VillageName == userdetails.Village).FirstOrDefault().Id : 0;
                Usersdata.Grampanchayat = userdetails.Grampanchayat != null ? db.Grampanchayats.Where(a => a.GrampanchayatName == userdetails.Grampanchayat).FirstOrDefault().Id : 0;
                Usersdata.Role = userdetails.Role != null ? db.Roles.Where(a => a.RoleName == userdetails.Role).FirstOrDefault().Id : 0;
                Usersdata.Aadhaar = userdetails.Aadhaar;
                Usersdata.IMEI1 = userdetails.IMEI1;
                Usersdata.IMEI2 = userdetails.IMEI2;
                Usersdata.Language = userdetails.Language != null ? db.Languages.Where(a => a.LanguageName == userdetails.Language).FirstOrDefault().Id : 0;
                Usersdata.FCMToken = userdetails.FCMToken;
                Usersdata.CreatedBy = Convert.ToInt32(userdetails.CreatedBy);
                UserCreateBy = Convert.ToInt32(userdetails.CreatedBy);
                Usersdata.CreatedOn = Convert.ToDateTime(userdetails.CreatedOn);
                UserCreatedOn = Convert.ToDateTime(userdetails.CreatedOn);
                Usersdata.BulkUploadId = userdetails.BulkUploadId;
                bulkUploadId = userdetails.BulkUploadId;
                Usersdata.UserId = userdetails.UserName;
                UsersList.Add(Usersdata);
            }

            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            foreach (var userData in UsersList)
            {
                db.Users.Add(userData);
                db.SaveChanges();
            }

            CreateBulkref(bulkUploadId, UserCreateBy, UserCreatedOn);
            
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { userList }, success = true, error = string.Empty });
        }

        public void CreateUserCrednetails(User user)
        {
            UserCredential loginUser = new UserCredential
            {
                UserId = user.Id,
                UserName = user.UserId,
                PhoneNumber = user.PhoneNumber,
                Password = "12345"
            };
            db.UserCredentials.Add(loginUser);
            db.SaveChanges();
        }

        public void CreateBulkref(string bulkUploadId, int UserCreateBy, DateTime UserCreatedOn)
        {
            BulkUploadRef bulkref = new BulkUploadRef
            {
                BulkUploadId = bulkUploadId,
                CreatedBy = UserCreateBy,
                CreatedOn = UserCreatedOn
            };
            db.BulkUploadRefs.Add(bulkref);
            db.SaveChanges();
        }

        public void CreateUserCredentails(string UserName, int UserCreateBy, DateTime UserCreatedOn)
        {
            BulkUploadRef bulkref = new BulkUploadRef
            {
                BulkUploadId = bulkUploadId,
                CreatedBy = UserCreateBy,
                CreatedOn = UserCreatedOn
            };
            db.BulkUploadRefs.Add(bulkref);
            db.SaveChanges();
        }


        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}