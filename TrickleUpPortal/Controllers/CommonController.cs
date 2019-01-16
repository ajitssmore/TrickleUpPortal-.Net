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
        string AudioFilePath;

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

        public Language fetchLang(int? LangId)
        {
            Language languageData = db.Languages.Find(LangId);
            return languageData;
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

        public string fetchAudioPahtSteps(int StepId, int langId, string fieldType)
        {
            var results = (from Audiodata in db.CropStepAudio_Allocation
                           join AudioFile in db.Audios on Audiodata.AudioId equals AudioFile.Id
                           where Audiodata.StepId == StepId && Audiodata.LangId == langId && Audiodata.FieldType == fieldType
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

        public string SendPushNotification(PushNotificationDataModel PushNotificationData)
        {
            string response;

            try
            {
                //var UserFCMTokendata = db.UserFCMTokens.Where(x => x.Registered == true).Select(a => new { a.FCMToken }).ToList();
                string[] deviceIDs = db.UserFCMTokens
                 .Where(x => x.Registered == true)
                 .Select(x => x.FCMToken).ToArray();
                string serverKey = "AAAABp-OKPA:APA91bGsf8As5tEhemJ1GRIBtEs4hy2OSzY9YcbLoaUNzdghuQkH7Fdnh3m6gUwXt1QbbNddFeTHenJkMsLCse_4kLL4z4UBMGv1hgwE9YkG4S9_tFY0wgYxR6Y8k43N90taY5sdKpIi";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                string[] tokens = deviceIDs;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                PushNotificationDataModel PushJsonData = new PushNotificationDataModel();
                PushJsonData.CropId = PushNotificationData.CropId;
                PushJsonData.StepId = PushNotificationData.StepId;
                PushJsonData.VideoURL = PushNotificationData.VideoURL;
                PushJsonData.Title = PushNotificationData.CropName + "\n" + PushNotificationData.StepName;
                PushJsonData.Body = string.Empty;
                PushJsonData.StepImageURL = PushNotificationData.StepImageURL;
                PushJsonData.CreatedOn = PushNotificationData.CreatedOn;
                string jsondata = JsonConvert.SerializeObject(PushJsonData);
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
            string CropName = "", StepName="";
            switch (NotificationModelData.notificationContext)
            {
                case "Crop":
                        CropName = db.Crops.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.CropName).Single();
                        objPushNotification.Title = !NotificationModelData.Active ?  ""+ NotificationModelData.category + " has been uploaded" : "" + NotificationModelData.category + " has been Removed";
                        objPushNotification.Body = "For" + " " + CropName;
                        objPushNotification.CropName = CropName;
                        objPushNotification.CropId = Convert.ToInt32(NotificationModelData.contextId);
                        objPushNotification.LangCode = Convert.ToInt32(NotificationModelData.languageId);
                    break;
                case "CropSteps":
                    StepName = db.Cultivation_Steps.Where(x => x.Id == NotificationModelData.contextId).Select(x => x.Step_Name).Single();
                    objPushNotification.Title = !NotificationModelData.Active ? "" + NotificationModelData.category + " has been uploaded" : "" + NotificationModelData.category + " has been Removed";
                    objPushNotification.Body = "For" + " " + CropName + " -->" + " " + StepName;
                    break;
                case "CropStepMaterial":
                    Console.WriteLine("Well done");
                    break;
                default:
                    break;
            }

            //objPushNotification.CropName = CropName;
            //objPushNotification.StepName = StepName;
            //objPushNotification.CropId = Cultivation_StepsData.Crop_Id;
            //objPushNotification.StepId = Cultivation_StepsData.Id;
            //objPushNotification.LangCode = LanguageCode;
            //objPushNotification.VideoURL = cultivation_Steps.VideoPath;
            //objPushNotification.StepImageURL = Cultivation_StepsData.ImagePath;
            //objPushNotification.CreatedOn = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            //string message = comObj.SendPushNotification(objPushNotification);
            //objPushNotification.ResponseMessage = message;
            //StoreNotificationData(objPushNotification);

            return (HttpResponseMessage)Request.CreateResponse(HttpStatusCode.OK, new { string.Empty, success = true, error = string.Empty });
        }

    }
}
