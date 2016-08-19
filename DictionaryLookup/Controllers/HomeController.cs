using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DictionaryLookup.Models;
using System.Text;

namespace DictionaryLookup.Controllers
{
    public class HomeController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: Home0
        public ActionResult Index(string word, string prefix, string version)
        {
            int numToReturn = 100;
            Int32 versionID = string.IsNullOrEmpty(version)?1:Int32.Parse(version);
            string previous1 = "";
            string previous2 = "";

            if (!String.IsNullOrEmpty(word))
                word = word.Trim();
            if (!String.IsNullOrEmpty(prefix))
                prefix = prefix.Trim();
            if (!String.IsNullOrEmpty(prefix))
            {
                if (prefix.Contains(" "))
                {
                    previous1 = prefix.Substring(prefix.IndexOf(' ') + 1);
                    previous2 = prefix.Remove(prefix.IndexOf(' '));
                }
                else
                {
                    previous1 = prefix;
                }
            }

            StringBuilder sqlcmd = new StringBuilder("SELECT top " + numToReturn.ToString() +
                " nge.NGramEntryID AS DictionaryNGramID" +
                ", (ISNULL(pw2.Word + ' ', '') + ISNULL(pw1.Word + ' ', '') + cw1.Word) AS NGramWordString" +
                ", ngt.Restricted AS Restricted" +
                ", ngt.SpellerFrequency AS SpellerFrequency" +
                ", ngt.TextPredictionBadWord AS TextPredictionBadWord" +
                ", ngt.TextPredictionCost AS TextPredictionCost" +
                ", ngt.TextPredictionBackOffCost AS TextPredictionBackOffCost" +
                ", ngt.HWRCost AS HWRCost" +
                ", ngt.HWRCalligraphyCost AS HWRCalligraphyCost" +
                " FROM NGramEntries nge" +
                " JOIN NGramStrings ngs ON nge.NGramStringID = ngs.NGramStringID" +
                " JOIN WordStrings cw1 ON ngs.WordID = cw1.WordStringID" +
                " JOIN WordStrings pw1 ON ngs.Previous1WordID = pw1.WordStringID" +
                " JOIN WordStrings pw2 ON ngs.Previous2WordID = pw2.WordStringID" +
                " JOIN NGramTags ngt ON nge.NGramTagsID = ngt.NGramTagsID" +
                " WHERE nge.VersionedDictionaryID = " + versionID.ToString());

            if (!String.IsNullOrEmpty(word))
            {
                sqlcmd.Append(" AND cw1.Word = '" + word + "'");
            }
            if (!String.IsNullOrEmpty(previous1))
            {
                sqlcmd.Append(" AND pw1.Word = '" + previous1 + "'");
            }
            if (!String.IsNullOrEmpty(previous2))
            {
                sqlcmd.Append(" AND pw2.Word = '" + previous2 + "'");
            }

            sqlcmd.Append(" ORDER BY ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word");
            //IEnumerable<NGramViewModel> report = db.Database.SqlQuery<NGramViewModel>(sqlcmd.ToString());
            List<NGramViewModel> report = db.Database.SqlQuery<NGramViewModel>(sqlcmd.ToString()).ToList();
            return View(report);
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StringBuilder sqlcmd = new StringBuilder("SELECT top 1" +
                " nge.NGramEntryID AS DictionaryNGramID" +
                ", (ISNULL(pw2.Word + ' ', '') + ISNULL(pw1.Word + ' ', '') + cw1.Word) AS NGramWordString" +
                ", ngt.Restricted AS Restricted" +
                ", ngt.SpellerFrequency AS SpellerFrequency" +
                ", ngt.TextPredictionBadWord AS TextPredictionBadWord" +
                ", ngt.TextPredictionCost AS TextPredictionCost" +
                ", ngt.TextPredictionBackOffCost AS TextPredictionBackOffCost" +
                ", ngt.HWRCost AS HWRCost" +
                ", ngt.HWRCalligraphyCost AS HWRCalligraphyCost" +
                " FROM NGramEntries nge" +
                " JOIN NGramStrings ngs ON nge.NGramStringID = ngs.NGramStringID" +
                " JOIN WordStrings cw1 ON ngs.WordID = cw1.WordStringID" +
                " JOIN WordStrings pw1 ON ngs.Previous1WordID = pw1.WordStringID" +
                " JOIN WordStrings pw2 ON ngs.Previous2WordID = pw2.WordStringID" +
                " JOIN NGramTags ngt ON nge.NGramTagsID = ngt.NGramTagsID" +
                " WHERE nge.NGramEntryID = " + id.ToString());
            List<NGramViewModel> report = db.Database.SqlQuery<NGramViewModel>(sqlcmd.ToString()).ToList();

            return View(report.First());
        }
                        
        public ActionResult ReportError(Int64 ngramid, Int16 errorid)
        {
            DictionaryErrorReport dr = new DictionaryErrorReport(ngramid, errorid, 1, "");
            db.DictionaryErrorReports.Add(dr);
            db.SaveChanges();
            return RedirectToAction("Details", "DictionaryErrorReports", new { id = dr.DictionaryErrorReportId });
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
