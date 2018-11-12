using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrickleUpPortal.Models;

namespace TrickleUpPortal.Controllers
{
    public class CommonController : ApiController
    {
        private TrickleUpEntities db = new TrickleUpEntities();

        public string GetResxNameByValue_Hindi(string value)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("TrickleUpPortal.Resources.Lang_hindi", this.GetType().Assembly);
            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value.ToString().Trim());

            var key = entry.Key.ToString();
            return key;
        }

        public string GetResxNameByValue_Oriya(string value)
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager("TrickleUpPortal.Resources.Lang_Oriya", this.GetType().Assembly);
            var entry =
                rm.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value.ToString().Trim());

            var key = entry.Key.ToString();
            return key;
        }

        public Language fetchLang(int LangId)
        {
            Language languageData = db.Languages.Find(LangId);
            return languageData;
        }

    }
}
