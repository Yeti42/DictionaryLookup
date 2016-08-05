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
    public class DictionaryErrorReportsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: DictionaryErrorReports
        public ActionResult Index()
        {
            return View(db.DictionaryErrorReports.ToList());
        }

        // GET: DictionaryErrorReports/Details/5
        public ActionResult Details(Int32 id)
        {
            var errorReport = from r1 in db.DictionaryErrorReports
                              where r1.DictionaryErrorReportId == id
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
