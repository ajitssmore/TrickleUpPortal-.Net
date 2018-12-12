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

        public string SendPushNotification(string Title, string body)
        {
            string response;

            try
            {
                //var UserFCMTokendata = db.UserFCMTokens.Where(x => x.Registered == true).Select(a => new { a.FCMToken }).ToList();
                string[] deviceIDs = db.UserFCMTokens
                 .Where(x => x.Registered == true)
                 .Select(x => x.FCMToken).ToArray();
                //dynamic FCMTokanList = new List<ExpandoObject>();
                //foreach (var item in UserFCMTokendata)
                //{
                //    dynamic ToaknData = new ExpandoObject();
                //    ToaknData.Tokan = item.FCMToken;
                //    FCMTokanList.Add(ToaknData);
                //}
                //string[] DeviceToken = FCMTokanList.ToArray();

                string serverKey = "AAAABp-OKPA:APA91bGsf8As5tEhemJ1GRIBtEs4hy2OSzY9YcbLoaUNzdghuQkH7Fdnh3m6gUwXt1QbbNddFeTHenJkMsLCse_4kLL4z4UBMGv1hgwE9YkG4S9_tFY0wgYxR6Y8k43N90taY5sdKpIi";
                //string deviceId1 = "fgTG8dtVpEE:APA91bE6_0gM7Wx9_BKYKEU8z4MPIGiNZ5nNLhG7BupHnVn69X_JfCJLZwqSdtS9bS56MFv3_OMJdUppVqdd2WMwPTjlulkE1O3TtMxoFQntpn1pWU43eS3EQXxnz0En4prAkiU36TrM";
                //string deviceId2 = "cKF7bsNh7ow:APA91bEMRt1c9q4bH_Xbkj8V2VFcQ-UfdUX3Ba158KLyJd-U67DR-kkF8B_f1ftc00eAAB2X5KoO_P1KWNfMgcZhcMThGBvXZFUzVuaK6bZ70AOLIL1Cg9F-Jbe11qCNF3a8PDdNIVko";
                //string deviceId3 = "dLMbAcX_TCQ:APA91bG5AbjX6f2CJpfMrkh_cW6tA7vUFXMEHoUv9KqOelL09FMtTTFsAdl0dtYAtbseQtzWaCkUqapT0qMXvw-lohsamiEwGWGUzDRnK4bdxlF6TZRZrwYnr-3frlwUdX40BS-V16MI";
                //string deviceId4 = "esBy7STCS68:APA91bGZoiWrVbt8KaROBzz-O68H_jaZw2o1wdqsHz77fzeddIjJqLI8oRADvrn5ROrkIA7zlpLHeM51ft8ZTyMe2-w6JAwGLl-22iQCiYlSGht3XexbuhSzfM3fsDXl3zgYn27NMaS_";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                //string[] tokens = { deviceId1, deviceId2, deviceId3, deviceId4 };
                string[] tokens = deviceIDs;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var message = new
                {
                    registration_ids = tokens,
                    notification = new
                    {
                        body = "Greeting from Trickle up - Snehal Patil",
                        title = "Hello Eternus from Notification Trial 14.46",
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

    }
}
