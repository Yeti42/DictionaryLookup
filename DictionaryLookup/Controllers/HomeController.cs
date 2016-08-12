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
            if (!string.IsNullOrEmpty(prefix) && prefix.Contains(" "))
            {
                previous1 = prefix.Substring(prefix.IndexOf(' ') + 1);
                previous2 = prefix.Remove(prefix.IndexOf(' '));
            }

            if (!String.IsNullOrEmpty(word))
            {
                if (!String.IsNullOrEmpty(previous2))
                {
                    var report = (from nge in db.NGramEntries
                                  join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                                  join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                                  join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                                  join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                                  join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                                  where nge.VersionedDictionaryID == versionID
                                  where cw1.Word == word
                                  where pw1.Word == previous1
                                  where pw2.Word == previous2
                                  orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                                  select new NGramViewModel
                                  {
                                      NGramWordString = String.Concat(pw2.Word, " ", pw1.Word, " ", cw1.Word),
                                      Tags = ngt,
                                      DictionaryNGramID = nge.NGramEntryID
                                  }).Take(numToReturn);
                    return View(report.ToList());
                }
                else if (!String.IsNullOrEmpty(previous1))
                {
                    var report = (from nge in db.NGramEntries
                                  join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                                  join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                                  join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                                  join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                                  join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                                  where nge.VersionedDictionaryID == versionID
                                  where cw1.Word == word
                                  where pw1.Word == previous1
                                  orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                                  select new NGramViewModel
                                  {
                                      NGramWordString = String.Concat(pw2.Word, " ", pw1.Word, " ", cw1.Word),
                                      Tags = ngt,
                                      DictionaryNGramID = nge.NGramEntryID
                                  }).Take(numToReturn);
                    return View(report.ToList());

                }
                else
                {
                    var report = (from nge in db.NGramEntries
                                  join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                                  join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                                  join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                                  join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                                  join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                                  where nge.VersionedDictionaryID == versionID
                                  where cw1.Word == word
                                  orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                                  select new NGramViewModel
                                  {
                                      NGramWordString = String.Concat(pw2.Word, " ", pw1.Word, " ", cw1.Word),
                                      Tags = ngt,
                                      DictionaryNGramID = nge.NGramEntryID
                                  }).Take(numToReturn);
                    return View(report.ToList());

                }
            }
            else if (!String.IsNullOrEmpty(previous2))
            {
                var report = (from nge in db.NGramEntries
                              join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                              join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                              join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                              join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                              join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                              where nge.VersionedDictionaryID == versionID
                              where pw1.Word == previous1
                              where pw2.Word == previous2
                              orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                              select new NGramViewModel
                              {
                                  NGramWordString = String.Concat(pw2.Word, " ", pw1.Word, " ", cw1.Word),
                                  Tags = ngt,
                                  DictionaryNGramID = nge.NGramEntryID
                              }).Take(numToReturn);
                return View(report.ToList());

            }
            else if (!String.IsNullOrEmpty(previous1))
            {
                var report = (from nge in db.NGramEntries
                              join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                              join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                              join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                              join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                              join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                              where nge.VersionedDictionaryID == versionID
                              where pw1.Word == previous1
                              orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                              select new NGramViewModel
                              {
                                  NGramWordString = String.Concat(pw2.Word, " ", pw1.Word, " ", cw1.Word),
                                  Tags = ngt,
                                  DictionaryNGramID = nge.NGramEntryID
                              }).Take(numToReturn);
                return View(report.ToList());

            }
            else
            {
                List<NGramViewModel> ngvm = new List<NGramViewModel>();
                return View(ngvm);
                /*
                var report = (from nge in db.NGramEntries
                              join ngs in db.NGramStrings on nge.NGramStringID equals ngs.NGramStringID
                              join cw1 in db.WordStrings on ngs.WordID equals cw1.WordStringID
                              join pw1 in db.WordStrings on ngs.Previous1WordID equals pw1.WordStringID
                              join pw2 in db.WordStrings on ngs.Previous2WordID equals pw2.WordStringID
                              join ngt in db.NGramTags on nge.NGramTagsID equals ngt.NGramTagsID
                              where nge.VersionedDictionaryID == versionID
                              orderby ngs.NGram, ngt.TextPredictionCost, cw1.Word, pw1.Word, pw2.Word
                              select new NGramViewModel
                              {
                                  NGramWordString = pw2.Word + " " + pw1.Word + " " + cw1.Word,
                                  Tags = ngt,
                                  DictionaryNGramID = nge.NGramEntryID
                              }).Take(numToReturn);
                return View(report.ToList());
                */

            }



            /*
            if (!String.IsNullOrEmpty(word))
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    string sqlcmd = "SELECT top " + numToReturn.ToString() + " * FROM DictionaryWords" +
                                    " WHERE Word LIKE @p0 + ' ' + @p1" +
                                    " OR Word LIKE '% ' + @p0 + ' ' + @p1" +
                                    " ORDER BY NGram, TextPredictionCost, Word";
                    return View(db.DictionaryWords.SqlQuery(sqlcmd, prefix, word).ToList());
                }
                else
                {
                    string sqlcmd = "SELECT top " + numToReturn.ToString() + " * FROM DictionaryWords" +
                                    " WHERE Word LIKE @p0" +
                                    " OR Word LIKE '% ' + @p0" +
                                    " ORDER BY NGram, TextPredictionCost, Word";
                    return View(db.DictionaryWords.SqlQuery(sqlcmd, word).ToList());
                }
            }
            else if (!string.IsNullOrEmpty(prefix))
            {
                string sqlcmd = "SELECT top " + numToReturn.ToString() + " * FROM DictionaryWords" +
                                " WHERE Word LIKE @p0 + ' %'" +
                                " ORDER BY NGram, TextPredictionCost, Word";
                return View(db.DictionaryWords.SqlQuery(sqlcmd, prefix).ToList());
            }
            */
            //var dbreport = db.NGramEntries.SqlQuery(sqlcmd);
            //var dbreport2 = db.NGramEntries.SqlQuery(sqlcmd).ToList();

            //return View(report.ToList());

            //return View(words);

        }

        /*
        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictionaryWord dictionaryWord = db.DictionaryWords.Find(id);
            if (dictionaryWord == null)
            {
                return HttpNotFound();
            }

            return View(dictionaryWord);
        }
        */
                        
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
