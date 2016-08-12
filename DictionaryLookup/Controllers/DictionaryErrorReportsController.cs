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
            var errorReport = from er1 in db.DictionaryErrorReports
                              join et1 in db.ErrorTypes on er1.ErrorTypeID equals et1.ErrorTypeID
                              join us1 in db.Users on er1.UserID equals us1.UserID
                              join nge in db.NGramEntries on er1.NGramEntryID equals nge.NGramEntryID
                              join vd1 in db.VersionedDictionaries on nge.VersionedDictionaryID equals vd1.VersionedDictionaryID
                              join lg1 in db.Languages on vd1.LanguageID equals lg1.LanguagesID
                              join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                              join wd1 in db.WordStrings on ngs.WordID equals wd1.WordStringID
                              join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                              join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                              orderby er1.DictionaryErrorReportId descending
                              select new ErrorReportViewModel { dr = er1.DictionaryErrorReportId, errorName = et1.ErrorTypeName, userName = us1.UserContact, languageName = lg1.FriendlyName, nGramString = pw2.Word + " " + pw1.Word + " " + wd1.Word, reportDateTime = er1.ReportDateTime, notes = er1.Notes };
            return View(errorReport.ToList());
        }

        // GET: DictionaryErrorReports/Details/5
        public ActionResult Details(Int32 id)
        {
            var errorReport = from er1 in db.DictionaryErrorReports
                               where er1.DictionaryErrorReportId == id
                               join et1 in db.ErrorTypes on er1.ErrorTypeID equals et1.ErrorTypeID
                               join us1 in db.Users on er1.UserID equals us1.UserID
                               join nge in db.NGramEntries on er1.NGramEntryID equals nge.NGramEntryID
                               join vd1 in db.VersionedDictionaries on nge.VersionedDictionaryID equals vd1.VersionedDictionaryID
                               join lg1 in db.Languages on vd1.LanguageID equals lg1.LanguagesID
                               join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                               join wd1 in db.WordStrings on ngs.WordID equals wd1.WordStringID
                               join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                               join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                               select new { er1, et1.ErrorTypeName, us1.UserContact, lg1.FriendlyName, s1 = pw2.Word + " " + pw1.Word + " " + wd1.Word };

            return View(new ErrorReportViewModel(errorReport.First().er1.DictionaryErrorReportId,
                                                 errorReport.First().ErrorTypeName,
                                                 errorReport.First().UserContact,
                                                 errorReport.First().FriendlyName,
                                                 errorReport.First().er1.ReportDateTime,
                                                 errorReport.First().s1,
                                                 errorReport.First().er1.Notes));
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
