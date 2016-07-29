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
    public class WordStringsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: WordStrings
        public ActionResult Index()
        {
            return View(db.WordStrings.ToList());
        }
        
        // GET: WordStrings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordString wordString = db.WordStrings.Find(id);
            if (wordString == null)
            {
                return HttpNotFound();
            }
            return View(wordString);
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
