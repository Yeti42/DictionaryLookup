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
        public ActionResult Index(string word, string prefix)
        {
            int numToReturn = 40;
            /*
            if (!String.IsNullOrEmpty(word))
                word = word.Trim();
            if (!string.IsNullOrEmpty(prefix))
                prefix = prefix.Trim();
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
            return View(db.WordStrings.SqlQuery("SELECT top " + numToReturn.ToString() + " FROM WordStrings ORDER BY WordString").ToList());

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
                        
        public ActionResult ReportError(Int64 wordid, Int16 errorid)
        {
            DictionaryErrorReport dr = new DictionaryErrorReport(wordid, errorid, 1, "");
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
