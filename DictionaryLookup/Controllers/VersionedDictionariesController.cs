using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DictionaryLookup.Models;
using System.IO;
using System.Collections;
using System.Threading;

namespace DictionaryLookup.Controllers
{
    public class VersionedDictionariesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: VersionedDictionaries
        public ActionResult Index()
        {
            return View(db.VersionedDictionaries.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string inputFilename;
    }

}
