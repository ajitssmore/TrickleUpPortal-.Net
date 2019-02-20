using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class CommonController : ApiController
    {
        public TrickleUpEntities db = new TrickleUpEntities();
        string AudioFilePath, LanguageName;

        public string GetResxNameByValue_Hindi(string value)
        {
            string key = "";
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("TrickleUpPortal.Resources.Lang_hindi", this.GetType().Assembly);
            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value.Trim());
            if (entry.Key != null)
            {
                key = entry.Key.ToString();
            }
            else
            {
                key = value;
            }
            return key;
        }

        public string GetResxNameByValue_Oriya(string value)
        {
            string key="";
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("TrickleUpPortal.Resources.Lang_Oriya", this.GetType().Assembly);
            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value.ToString().Trim());

            if (entry.Key != null)
            {
                key = entry.Key.ToString();
            }
            else
            {
                key = value;
            }
            return key;
        }

        //public Language fetchLang(int? LangId)
        //{
        //    Language languageData = db.Languages.Find(LangId);
        //    return languageData;
        //}

        public string fetchLang(int? LangId)
        {
            Language languageData = db.Languages.Find(LangId);
            if (languageData != null)
            {
                LanguageName = languageData.LanguageName;
            }
            else
            {
                LanguageName = db.Languages.Where(a => a.LanguageName == "English").Select(a => a.LanguageName).FirstOrDefault().ToString();
            }
            return LanguageName;
        }

        public string fetchAudioPahtCrops(int CropId, int langId)
        {
            var results = (from Audiodata in db.Crop_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.CropId == CropId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtLiveStock(int LiveStockId, int langId)
        {
            var results = (from Audiodata in db.LiveStock_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.LiveStockId == LiveStockId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtLiveStockBreed(int LiveStockBreedId, int langId)
        {
            var results = (from Audiodata in db.LiveStockBreed_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.LiveStockBreedId == LiveStockBreedId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtLiveStockSteps(int LiveStockStepsId, int langId)
        {
            var results = (from Audiodata in db.LiveStock_Steps_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.LiveStockStepId == LiveStockStepsId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtLiveStockStepsMaterial(int MaterialId, int langId, string fieldType)
        {
            var results = (from Audiodata in db.LiveStock_StepsMaterial_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.LiveStockStepMaterialId == MaterialId && Audiodata.LangId == langId && Audiodata.FieldType == fieldType && Audiodata.Active == true
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        case "Description":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtLiveStockBreedCategory(int LiveStockBreedCategoryId, int langId)
        {
            var results = (from Audiodata in db.LiveStock_BreedCategory_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.LiveStockBreedCategoryId == LiveStockBreedCategoryId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtSteps(int StepId, int langId, string fieldType)
        {
            var results = (from Audiodata in db.CropStepAudio_Allocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.StepId == StepId && Audiodata.LangId == langId && Audiodata.FieldType == fieldType && Audiodata.Active == true
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        case "Description":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public string fetchAudioPahtMaterials(int MaterialId, int langId)
        {
            var results = (from Audiodata in db.CropMaterial_AudioAllocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.MaterialId == MaterialId && Audiodata.LangId == langId
                           select new { AudioFile.FilePath, Audiodata.FieldType }).ToList();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    switch (item.FieldType)
                    {
                        case "Title":
                            AudioFilePath = item.FilePath;
                            break;
                        default:
                            AudioFilePath = string.Empty;
                            break;
                    }
                }
            }
            else
            {
                AudioFilePath = string.Empty;
            }
            return AudioFilePath;
        }

        public long SendExcepToDB(Exception exdb)
        {
            Tbl_ExceptionLogging tblExce = new Tbl_ExceptionLogging();
            tblExce.ExceptionMsg = exdb.Message.ToString();
            tblExce.ExceptionType = exdb.GetType().Name.ToString();
            tblExce.ExceptionURL = exdb.TargetSite.Name.ToString();
            tblExce.ExceptionSource = exdb.StackTrace.ToString();
            tblExce.Logdate = DateTime.Now;
            Tbl_ExceptionLoggingController Tbl_ExceptionLogging = new Tbl_ExceptionLoggingController();
            long Exceptionid = Tbl_ExceptionLogging.SaveTbl_ExceptionLogging(tblExce);
            return Exceptionid;
        }

        public string SendPushNotification(PushNotificationDataModel PushNotificationData, string[] deviceIDs)
        {
            string response;

            try
            {
                //var UserFCMTokendata = db.UserFCMTokens.Where(x => x.Registered == true).Select(a => new { a.FCMToken }).ToList();
                //string[] deviceIDs = db.UserFCMTokens
                // .Where(x => x.Registered == true)
                // .Select(x => x.FCMToken).ToArray();
                string serverKey = "AAAABp-OKPA:APA91bGsf8As5tEhemJ1GRIBtEs4hy2OSzY9YcbLoaUNzdghuQkH7Fdnh3m6gUwXt1QbbNddFeTHenJkMsLCse_4kLL4z4UBMGv1hgwE9YkG4S9_tFY0wgYxR6Y8k43N90taY5sdKpIi";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                string[] tokens = deviceIDs;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                string jsondata = JsonConvert.SerializeObject(PushNotificationData);
                var message = new
                {
                    registration_ids = tokens,
                    notification = new
                    {
                        body = PushNotificationData.Body,
                        title = PushNotificationData.Title,
                    },
                    data = new
                    {
                        jsondata
                    }

                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(message);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public HttpResponseMessage SendNotification(NotificationModel NotificationModelData)
        {
            PushNotificationDataModel objPushNotification = new PushNotificationDataModel();
            string CropName = "", StepName="", MaterialName = "", LanguageName="";
            List<int> UserId;

            LanguageName = fetchLang(NotificationModelData.languageId);
            
            switch (NotificationModelData.notificationContext)
            {
                case "crop":
                        CropName = db.Crops.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.CropName).FirstOrDefault();
                        string titlemessage = NotificationModelData.Active ? "" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NotificationModelData.category.ToLower()) + " has been uploaded" : "" + NotificationModelData.category + " has been Removed";
                        switch (LanguageName)
                            {
                                case "Hindi":
                                    objPushNotification.Title = GetResxNameByValue_Hindi(titlemessage);
                                    objPushNotification.Body = GetResxNameByValue_Hindi(CropName) + " " + GetResxNameByValue_Hindi("For");
                                break;
                                case "English":
                                    objPushNotification.Title = titlemessage;
                                    objPushNotification.Body = "For" + " " + CropName;
                                break;
                                case "Oriya":
                                    objPushNotification.Title = GetResxNameByValue_Oriya(titlemessage);
                                    objPushNotification.Body = GetResxNameByValue_Oriya(CropName) + " " + GetResxNameByValue_Oriya("For");
                                break;
                                default:
                                    break;
                            }
                        objPushNotification.CropName = CropName;
                        objPushNotification.CropId = Convert.ToInt32(NotificationModelData.contextId);
                        objPushNotification.LangCode = Convert.ToInt32(NotificationModelData.languageId);
                        objPushNotification.notificationContext = NotificationModelData.notificationContext;
                        switch (NotificationModelData.category)
                        {
                            case "image":
                                    objPushNotification.ImageURL = db.Crops.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.FilePath).FirstOrDefault();
                                break;
                            case "audio":
                                    int AudioId = Convert.ToInt32(db.Crop_AudioAllocation.Where(x => x.CropId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.Active == true).Select(x => x.AudioId).FirstOrDefault());
                                    objPushNotification.AudioURL = AudioId!=0 ? db.Audios.Where(q => q.Id == AudioId).Select(q => q.FilePath).FirstOrDefault() : string.Empty;
                                break;
                            case "video":
                                int VideoId = Convert.ToInt32(db.Crop_VideoAllocation.Where(x => x.CropId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.Active == true).Select(x => x.VideoId).FirstOrDefault());
                                objPushNotification.VideoURL = VideoId != 0 ? db.Videos.Where(q => q.Id == VideoId).Select(q => q.FilePath).FirstOrDefault(): string.Empty; ;
                                break;
                            default:
                                break;
                        }
                    break;
                case "cropStep":
                    StepName = db.Cultivation_Steps.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.Step_Name).Single();
                    CropName = (from Cultivation_Step in db.Cultivation_Steps
                                join Crop in db.Crops on Cultivation_Step.Crop_Id equals Crop.Id
                                where Cultivation_Step.Id == NotificationModelData.contextId
                                select(Crop.CropName)).FirstOrDefault().ToString();

                        string titlemessageSteps = NotificationModelData.Active ? "" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NotificationModelData.category.ToLower()) + " has been uploaded" : "" + NotificationModelData.category + " has been Removed";
                        switch (LanguageName)
                        {
                            case "Hindi":
                                objPushNotification.Title = GetResxNameByValue_Hindi(titlemessageSteps);
                                objPushNotification.Body = GetResxNameByValue_Hindi(CropName) + " -->" + " " + GetResxNameByValue_Hindi(StepName) + " " + GetResxNameByValue_Hindi("For");
                                break;
                            case "English":
                                objPushNotification.Title = titlemessageSteps;
                                objPushNotification.Body = "For" + " " + CropName + " -->" + " " + StepName;
                                break;
                            case "Oriya":
                                objPushNotification.Title = GetResxNameByValue_Oriya(titlemessageSteps);
                                objPushNotification.Body = GetResxNameByValue_Oriya(CropName) + " -->" + " " + GetResxNameByValue_Oriya(StepName) + " " + GetResxNameByValue_Oriya("For");
                            break;
                            default:
                                break;
                        }

                    objPushNotification.StepId = Convert.ToInt32(NotificationModelData.contextId);
                    objPushNotification.CropId = db.Cultivation_Steps.Where(a => a.Id == NotificationModelData.contextId).Select(a => a.Crop_Id).FirstOrDefault();
                    objPushNotification.LangCode = Convert.ToInt32(NotificationModelData.languageId);
                    objPushNotification.notificationContext = NotificationModelData.notificationContext;
                    switch (NotificationModelData.category)
                    {
                        case "image":
                            objPushNotification.ImageURL = db.Cultivation_Steps.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.ImagePath).FirstOrDefault();
                            break;
                        case "audio":
                            int AudioId = Convert.ToInt32(db.CropStepAudio_Allocation.Where(x => x.StepId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.FieldType == NotificationModelData.fieldType && x.Active == true).Select(x => x.AudioId).FirstOrDefault());
                            objPushNotification.AudioURL = db.Audios.Where(q => q.Id == AudioId).Select(q => q.FilePath).Single();
                            break;
                        case "video":
                            int VideoId = Convert.ToInt32(db.CropStep_VideoAllocation.Where(x => x.StepId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.Active == true).Select(x => x.VideoId).FirstOrDefault());
                            objPushNotification.VideoURL = db.Videos.Where(q => q.Id == VideoId).Select(q => q.FilePath).Single();
                            break;
                        default:
                            break;
                    }
                    break;
                case "cropMaterial":
                    MaterialName = db.CropSteps_Material.Where(a => a.Id == NotificationModelData.contextId).Select(a => a.Material_Name).FirstOrDefault();
                    StepName = (from CropSteps_Materials in db.CropSteps_Material
                                join Cultivation_Step in db.Cultivation_Steps on CropSteps_Materials.Step_Id equals Cultivation_Step.Id
                                where CropSteps_Materials.Id == NotificationModelData.contextId
                                select(Cultivation_Step.Step_Name)).FirstOrDefault().ToString();
                    int stepId = db.CropSteps_Material.Where(b => b.Id == NotificationModelData.contextId).Select(b => b.Step_Id).FirstOrDefault();

                    CropName = (from Cultivation_Step in db.Cultivation_Steps
                                join Crop in db.Crops on Cultivation_Step.Crop_Id equals Crop.Id
                                where Cultivation_Step.Id == stepId
                                select(Crop.CropName)).FirstOrDefault().ToString();

                    string titlemessageMaterial = NotificationModelData.Active ? "" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NotificationModelData.category.ToLower()) + " has been uploaded" : "" + NotificationModelData.category + " has been Removed";
                    switch (LanguageName)
                    {
                        case "Hindi":
                            objPushNotification.Title = GetResxNameByValue_Hindi(titlemessageMaterial);
                            objPushNotification.Body = GetResxNameByValue_Hindi(CropName) + " -->" + " " + GetResxNameByValue_Hindi(StepName) + " -->" + " " + GetResxNameByValue_Hindi(MaterialName) + " " + GetResxNameByValue_Hindi("For");
                            break;
                        case "English":
                            objPushNotification.Title = titlemessageMaterial;
                            objPushNotification.Body = "For" + " " + CropName + " -->" + " " + StepName + " -->" + " " + MaterialName;
                            break;
                        case "Oriya":
                            objPushNotification.Title = GetResxNameByValue_Oriya(titlemessageMaterial);
                            objPushNotification.Body = GetResxNameByValue_Oriya(CropName) + " -->" + " " + GetResxNameByValue_Oriya(StepName) + " -->" + " " + GetResxNameByValue_Oriya(MaterialName) + " " + GetResxNameByValue_Oriya("For");
                            break;
                        default:
                            break;
                    }
                    objPushNotification.MaterialId = Convert.ToInt32(NotificationModelData.contextId);
                    objPushNotification.StepId = stepId;
                    objPushNotification.CropId = db.Cultivation_Steps.Where(a => a.Id == stepId).Select(a => a.Crop_Id).FirstOrDefault();
                    objPushNotification.LangCode = Convert.ToInt32(NotificationModelData.languageId);
                    objPushNotification.notificationContext = NotificationModelData.notificationContext;
                    switch (NotificationModelData.category)
                    {
                        case "image":
                            objPushNotification.ImageURL = db.CropSteps_Material.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.Image_Path).FirstOrDefault();
                            break;
                        case "audio":
                            int AudioId = Convert.ToInt32(db.CropMaterial_AudioAllocation.Where(x => x.MaterialId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.Active == true).Select(x => x.AudioId).FirstOrDefault());
                            objPushNotification.AudioURL = db.Audios.Where(q => q.Id == AudioId).Select(q => q.FilePath).FirstOrDefault();
                            break;
                        case "video":
                            int VideoId = Convert.ToInt32(db.CropStepMaterial_VideoAllocation.Where(x => x.MaterialId == NotificationModelData.contextId && x.LangId == objPushNotification.LangCode && x.Active == true).Select(x => x.VideoId).FirstOrDefault());
                            objPushNotification.VideoURL = db.Videos.Where(q => q.Id == VideoId).Select(q => q.FilePath).FirstOrDefault();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            //UserId = db.Users.Where(a => a.Language == NotificationModelData.languageId).Select(a=>a.Id).ToList();
            string StateCode = Convert.ToString(NotificationModelData.stateId);
            string DistrictCode = Convert.ToString(NotificationModelData.districtId);
            string GrampanchayatCode = Convert.ToString(NotificationModelData.grampanchayatId);

            UserId = db.Users.Where(a => a.Language == NotificationModelData.languageId && a.Active == true ||
            ((StateCode != "" && a.State == NotificationModelData.stateId) || (DistrictCode != "" && a.District == NotificationModelData.districtId) ||
                (GrampanchayatCode != "" && a.Grampanchayat == NotificationModelData.grampanchayatId) || (NotificationModelData.villageIdList.Count != 0 && NotificationModelData.villageIdList.Contains(a.Village.Value))
            )).Select(a => a.Id).ToList();

            //string[] deviceIDs = db.UserFCMTokens
            //     .Where(x => x.Registered == true)
            //     .Select(x => x.FCMToken).ToArray();

            string[] deviceIDs = db.UserFCMTokens
                 .Where(x => x.Registered == true && UserId.Contains(x.UserId.Value))
                 .Select(x => x.FCMToken).ToArray();

            if (deviceIDs.Length == 0)
            {
                return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty, success = false, error = "There are no any Users to send a notification" });
            }
            
            objPushNotification.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");
            objPushNotification.MediaType = NotificationModelData.category;
            objPushNotification.FieldType = NotificationModelData.fieldType;
            string message = SendPushNotification(objPushNotification, deviceIDs);
            objPushNotification.ResponseMessage = message;
            StoreNotificationData(objPushNotification);

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { data= new { message }, success = true, error = string.Empty });
            //return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { string.Empty, success = true, error = string.Empty });
        }

        public void StoreNotificationData(PushNotificationDataModel PushNotificationData)
        {
            PushNotification objPushNotification = new PushNotification();
            objPushNotification.PushNotificationTitle = PushNotificationData.Title;
            objPushNotification.PushNotificationBody = PushNotificationData.Body;
            //objPushNotification.PushNotificationData = "{CropId:" + PushNotificationData.CropId + ", StepId:" + PushNotificationData.StepId + ", langCode:" + PushNotificationData.LangCode + ", Message:" + PushNotificationData.ResponseMessage + " }";
            objPushNotification.PushNotificationData = JsonConvert.SerializeObject(PushNotificationData);
            objPushNotification.CreatedOn = System.DateTime.Now;
            db.PushNotifications.Add(objPushNotification);
            db.SaveChanges();
        }

    }
}
