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
    public class NGramEntriesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: NGramEntries
        public ActionResult Index()
        {
            return View(db.NGramEntries.ToList());
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
