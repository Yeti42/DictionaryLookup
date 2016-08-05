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
            var errorReport = from er1 in db.DictionaryErrorReports
                              where er1.DictionaryErrorReportId == id
                              join et1 in db.ErrorTypes on er1.ErrorTypeID equals et1.ErrorTypeID
                              join us1 in db.Users on er1.UserID equals us1.UserID
                              join vd1 in db.VersionedDictionaries on er1.VersionedDictionaryID equals vd1.VersionedDictionaryID
                              join lg1 in db.Languages on vd1.LanguageID equals lg1.LanguagesID
                              join ng1 in db.NGramStrings on er1.NGramStringID equals ng1.WordID
                              join wd1 in db.WordStrings on ng1.WordID equals wd1.WordStringID
                              join pw1 in db.WordStrings on ng1.Previous1WordID equals pw1.WordStringID
                              join pw2 in db.WordStrings on ng1.Previous2WordID equals pw2.WordStringID
                              select new { er1, et1.ErrorTypeName, us1.UserContact, lg1.FriendlyName, wd1};

            return View(new ErrorReportViewModel(errorReport.First().er1.DictionaryErrorReportId,
                                                 errorReport.First().ErrorTypeName,
                                                 errorReport.First().UserContact,
                                                 errorReport.First().FriendlyName,
                                                 errorReport.First().wd1.Word));
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
