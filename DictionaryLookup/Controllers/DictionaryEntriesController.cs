using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DictionaryLookup.Models;

namespace DictionaryLookup.Controllers
{
    public class DictionaryEntriesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: DictionaryEntries
        public ActionResult Index()
        {
            return View(db.DictionaryEntries.ToList());
        }

        // GET: DictionaryEntries/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryEntry dictionaryEntry = db.DictionaryEntries.Find(id);
            if (dictionaryEntry == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryEntry);
        }


        private Int64 ReadOrCreateDictionaryEntry(bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
            //var nid = from a in db.NGramTags
            //          where a.TextPredictionCost.Equals(stopCost)
            //          where a.TextPredictionBackOffCost.Equals(backoffCost)
            //          where a.HWRCalligraphyCost.Equals(hwrCallig)
            //          where a.HWRCost.Equals(hwrCost)
            //          where a.Restricted.Equals(restricted)
            //          where a.SpellerFrequency.Equals(frequency)
            //          where a.TextPredictionBadWord.Equals(badWord)
            //          select a.NGramTagsID;
            //if (nid.Count() > 0)
            //{
            //    return nid.First();
            //}
            //NGramTags ngt = new NGramTags(restricted, frequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);
            //db.NGramTags.Add(ngt);


            //return ngt.NGramTagsID;
            //Int64 nge = 0;
            //Int64 ngt = 0;
            //DictionaryEntry de = new DictionaryEntry { NGramEntryID = nge, NGramTagID = ngt };

            //var de = from a in db.DictionaryEntries
              //       where a.NGramEntryID.

            return 0;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
