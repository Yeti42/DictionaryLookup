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
        public ActionResult Details(Int32 id)
        {
            //DictionaryReport dictionaryReport = db.DictionaryReports.Find(id);
            //if (dictionaryReport == null)
            //{
            //    return HttpNotFound();
            //}

            //var errorReport = from o in db.Orders
            //                  join u in db.Users on o.UserId equals u.Id into ou
            //                  where o.UserId == uId
            //                  from o in ou.DefaultIfEmpty()
            //                  select o;

            var errorReport = from r1 in db.DictionaryReports
                              where r1.DictionaryReportId == id
                              join e1 in db.ErrorTypes on r1.ErrorTypeID equals e1.ErrorTypeID
                              join u1 in db.Users on r1.UserID equals u1.UserID
                              join l1 in db.Languages on r1.LanguageID equals l1.LanguagesID
                              join w1 in db.DictionaryWords on r1.WordID equals w1.DictionaryWordID
                              select new { r1, e1.ErrorTypeName, u1.UserContact, l1.FriendlyName, w1.Word};

            return View(new ErrorReportViewModel(errorReport.First().r1,
                                                 errorReport.First().ErrorTypeName,
                                                 errorReport.First().UserContact,
                                                 errorReport.First().FriendlyName,
                                                 errorReport.First().Word));
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
