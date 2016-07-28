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
    public class DictionaryReportsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: DictionaryReports
        public ActionResult Index()
        {
            return View(db.DictionaryReports.ToList());
        }

        // GET: DictionaryReports/Details/5
        public ActionResult Details(Int64 id)
        {
            DictionaryReport dictionaryReport = db.DictionaryReports.Find(id);
            if (dictionaryReport == null)
            {
                return HttpNotFound();
            }
            return View(dictionaryReport);
        }

        // GET: DictionaryReports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DictionaryReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictionaryReportId,WordID,LanguageID,DictionaryVersion,ReportDateTime,ReportType,UserID,Notes")] DictionaryReport dictionaryReport)
        {
            if (ModelState.IsValid)
            {
                db.DictionaryReports.Add(dictionaryReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictionaryReport);
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
