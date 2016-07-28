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
            if (!String.IsNullOrEmpty(word))
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    string sqlcmd = "SELECT top 40 * FROM DictionaryWords" +
                                    " WHERE Word LIKE @p0 + ' ' + @p1" +
                                    " OR Word LIKE '% ' + @p0 + ' ' + @p1" +
                                    " ORDER BY NGram, TextPredictionCost, Word";
                    return View(db.DictionaryWords.SqlQuery(sqlcmd, prefix, word).ToList());
                }
                else
                {
                    string sqlcmd = "SELECT top 40 * FROM DictionaryWords" +
                                    " WHERE Word LIKE @p0" +
                                    " OR Word LIKE '% ' + @p0" +
                                    " ORDER BY NGram, TextPredictionCost, Word";
                    return View(db.DictionaryWords.SqlQuery(sqlcmd, word).ToList());
                }
            }
            else if (!string.IsNullOrEmpty(prefix))
            {
                string sqlcmd = "SELECT top 40 * FROM DictionaryWords" +
                                " WHERE Word LIKE @p0 + ' %'" +
                                " ORDER BY NGram, TextPredictionCost, Word";
                return View(db.DictionaryWords.SqlQuery(sqlcmd, prefix).ToList());
            }
            return View(db.DictionaryWords.SqlQuery("SELECT top 40 * FROM DictionaryWords ORDER BY NGram, TextPredictionCost, Word").ToList());

            //return View(words);

        }

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

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictionaryWordID,Word,Restricted,Self")] DictionaryWord dictionaryWord)
        {
            if (ModelState.IsValid)
            {
                db.DictionaryWords.Add(dictionaryWord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictionaryWord);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictionaryWordID,Word,Restricted,Self")] DictionaryWord dictionaryWord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionaryWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dictionaryWord);
        }
        
        public ActionResult ReportError(Int64 wordid, Int16 errorid)
        {
            DictionaryReport dr = new DictionaryReport(wordid, errorid, 1, "");
            db.DictionaryReports.Add(dr);
            db.SaveChanges();
            return RedirectToAction("Details", "DictionaryReports", new { id = dr.DictionaryReportId });
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
