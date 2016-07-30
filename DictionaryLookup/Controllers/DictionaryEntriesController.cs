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
