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
    public class NGramTagsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: NGramTags
        public ActionResult Index()
        {
            return View(db.NGramTags.ToList());
        }

        // GET: NGramTags/Create
        public void Create(bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
        }

        private Int64 ReadOrCreateNGramTag(string word)
        {
            //var wid = from a in db.NGramTags
            //          where a.TextPredictionCost.Equals(word)
            //          select a.WordStringID;
            //if (wid.Count() > 0)
            //{
            //    return wid.First();
            //}
            //WordString ws = new WordString(word);
            //db.WordStrings.Add(ws);
            //return ws.WordStringID;

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
