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
        CommonController comObj = new CommonController();
        private TrickleUpEntities db = new TrickleUpEntities();

        // GET: api/Users
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        // GET: api/Users
        public HttpResponseMessage GetAllUsers()
        {

            try
            {
                //int value = 1 / int.Parse("0");
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
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = result, success = true, error = string.Empty });
            }
            catch (Exception ex)
            {
                long ExceptionId = comObj.SendExcepToDB(ex);
                Tbl_ExceptionLogging tbl_ExceptionLogging = db.Tbl_ExceptionLogging.Find(ExceptionId);
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { tbl_ExceptionLogging }, success = false, error = string.Empty });
            }
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

            var usersData = db.Users.Where(q => q.PhoneNumber == user.PhoneNumber).Any() ? db.Users.Where(p => p.PhoneNumber.ToUpper() == user.PhoneNumber).First() : null;
            if (usersData != null && usersData.Id != user.Id)
            {
                if (db.Users.Any(p => p.PhoneNumber == user.PhoneNumber))
                {
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Phone Number already Exist" });
                }
            }
            else
            {
                try
                {
                    User UserUpdateData = db.Users.Where(a => a.Id == user.Id).FirstOrDefault();
                    UserUpdateData.UserId = user.UserId;
                    UserUpdateData.Name = user.Name;
                    UserUpdateData.PhoneNumber = user.PhoneNumber;
                    UserUpdateData.Age = user.Age;
                    UserUpdateData.Gender = user.Gender;
                    UserUpdateData.State = user.State;
                    UserUpdateData.District = user.District;
                    UserUpdateData.Village = user.Village;
                    UserUpdateData.Grampanchayat = user.Grampanchayat;
                    UserUpdateData.Aadhaar = user.Aadhaar;
                    UserUpdateData.IMEI1 = user.IMEI1;
                    UserUpdateData.IMEI2 = user.IMEI2;
                    UserUpdateData.Role = user.Role;
                    UserUpdateData.Language = user.Language;
                    UserUpdateData.FCMToken = user.FCMToken;
                    UserUpdateData.UpdatedBy = user.UpdatedBy;
                    UserUpdateData.UpdatedOn = user.UpdatedOn;
                    UserUpdateData.Active = user.Active;
                    //UserUpdateData.ImagePath = user.ImagePath;
                    //UserUpdateData.BulkUploadId = user.BulkUploadId;
                    db.SaveChanges();
                    UpdateUserCrednetails(user);
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
            }
            
            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { user }, success = true, error = string.Empty });
        }


        [HttpPost]
        public HttpResponseMessage ActiveDeactiveUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (id != user.Id)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { string.Empty }, success = false, error = string.Empty });
            }
            
            try
            {
                User UserUpdateData = db.Users.Where(a => a.Id == user.Id).FirstOrDefault();
                UserUpdateData.ActiveBy = user.ActiveBy;
                UserUpdateData.ActiveOn = user.ActiveOn;
                UserUpdateData.Active = user.Active;
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
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = string.Empty });
            }

            if (user.PhoneNumber != null || user.PhoneNumber != "")
            {
                int PhoneCount = db.Users.Where(a => a.PhoneNumber == user.PhoneNumber).ToList().Count;
                if (PhoneCount > 0)
                { 
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Phone Number already Exist" });
                }
            }

            if (!string.IsNullOrEmpty(user.UserId))
            {
                int UserIdCount = db.Users.Where(a => a.UserId == user.UserId).ToList().Count();
                if (UserIdCount > 0)
                { 
                    return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = new { string.Empty }, success = false, error = "Email already Exist" });
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
                if (string.IsNullOrEmpty(userdetails.Name))
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append("Beneficiary Name can't be Empty");
                }

                if (string.IsNullOrEmpty(userdetails.PhoneNumber) || userdetails.PhoneNumber.Length != 10)
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append(", Phone Number is invalid");
                }
                else if (userdetails.PhoneNumber != null)
                {
                    int PhoneCount = db.Users.Where(a => a.PhoneNumber == userdetails.PhoneNumber).ToList().Count;
                    if (PhoneCount > 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", Phone Number already exists");
                    }
                }

                if (!string.IsNullOrEmpty(userdetails.UserName))
                {
                    int UserIdCount = db.Users.Where(a => a.UserId == userdetails.UserName).ToList().Count();
                    if (UserIdCount > 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", Email Id already exists");
                    }
                }

                if (!string.IsNullOrEmpty(userdetails.State))
                {
                    int StateCount = db.States.Where(a => a.StateName.ToUpper() == userdetails.State.ToUpper()).ToList().Count();
                    if (StateCount == 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", State Does not exists");
                    }
                }
                else
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append(", State can't be null");
                }

                if (!string.IsNullOrEmpty(userdetails.District))
                {
                    int DistrictCount = db.Districts.Where(a => a.DistrictName.ToUpper() == userdetails.District.ToUpper()).ToList().Count();
                    if (DistrictCount == 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", District Does not exists");
                    }
                }
                else
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append(", District can't be null");
                }

                if (!string.IsNullOrEmpty(userdetails.Grampanchayat))
                {
                    int GrampanchayatCount = db.Grampanchayats.Where(a => a.GrampanchayatName.ToUpper() == userdetails.Grampanchayat.ToUpper()).ToList().Count();
                    if (GrampanchayatCount == 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", Grampanchayat Does not exists");
                    }
                }
                else
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append(", Grampanchayat can't be null");
                }

                if (!string.IsNullOrEmpty(userdetails.Village))
                {
                    int VillageCount = db.Villages.Where(a => a.VillageName.ToUpper() == userdetails.Village.ToUpper()).ToList().Count();
                    if (VillageCount == 0)
                    {
                        ErrorMesFlag = true;
                        ErrorMerssage.Append(", Village Does not exists");
                    }
                }
                else
                {
                    ErrorMesFlag = true;
                    ErrorMerssage.Append(", Village can't be null");
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
                Usersdata.Name = !string.IsNullOrEmpty(userdetails.Name) ? userdetails.Name : null;
                Usersdata.UserId = !string.IsNullOrEmpty(userdetails.UserName) ? userdetails.UserName : null;
                Usersdata.PhoneNumber = !string.IsNullOrEmpty(userdetails.PhoneNumber) ? userdetails.PhoneNumber : null; 
                Usersdata.Age = !string.IsNullOrEmpty(userdetails.Age) ? Convert.ToInt32(userdetails.Age) : 0; 
                Usersdata.Gender = !string.IsNullOrEmpty(userdetails.Gender) && db.Genders.Where(a => a.GenderName == userdetails.Gender).Any() ? db.Genders.Where(a => a.GenderName == userdetails.Gender).FirstOrDefault().Id: (int?)null;
                Usersdata.State = !string.IsNullOrEmpty(userdetails.State) && db.States.Where(a => a.StateName == userdetails.State).Any() ? db.States.Where(a => a.StateName == userdetails.State).FirstOrDefault().Id : (int?)null;
                Usersdata.District = !string.IsNullOrEmpty(userdetails.District) && db.Districts.Where(a => a.DistrictName == userdetails.District).Any() ? db.Districts.Where(a => a.DistrictName == userdetails.District).FirstOrDefault().Id : (int?)null;
                Usersdata.Village = !string.IsNullOrEmpty(userdetails.Village) && db.Villages.Where(a => a.VillageName == userdetails.Village).Any() ? db.Villages.Where(a => a.VillageName == userdetails.Village).FirstOrDefault().Id : (int?)null;
                Usersdata.Grampanchayat = !string.IsNullOrEmpty(userdetails.Grampanchayat) && db.Grampanchayats.Where(a => a.GrampanchayatName == userdetails.Grampanchayat).Any() ? db.Grampanchayats.Where(a => a.GrampanchayatName == userdetails.Grampanchayat).FirstOrDefault().Id : (int?)null;
                //Usersdata.Role = !string.IsNullOrEmpty(userdetails.Role) && db.Roles.Where(a => a.RoleName == userdetails.Role).Any() ? db.Roles.Where(a => a.RoleName == userdetails.Role).FirstOrDefault().Id : (int?)null;
                Usersdata.Role = db.Roles.Where(a => a.RoleName == "Beneficiary User").FirstOrDefault().Id;
                Usersdata.Aadhaar = userdetails.Aadhaar;
                Usersdata.IMEI1 = userdetails.IMEI1;
                Usersdata.IMEI2 = userdetails.IMEI2;
                Usersdata.Language = !string.IsNullOrEmpty(userdetails.Language) && db.Languages.Where(a => a.LanguageName == userdetails.Language).Any() ? db.Languages.Where(a => a.LanguageName == userdetails.Language).FirstOrDefault().Id : (int?)null;
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
                Password = !string.IsNullOrEmpty(user.Password) ? user.Password : "12345"
            };
            db.UserCredentials.Add(loginUser);
            db.SaveChanges();
        }

        public void UpdateUserCrednetails(User user)
        {
            UserCredential Userdata = db.UserCredentials.Where(a => a.UserId == user.Id).FirstOrDefault();
            Userdata.UserName = user.UserId;
            Userdata.PhoneNumber = user.PhoneNumber;
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