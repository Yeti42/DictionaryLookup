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

            /*
            List<DictionaryWord> words = new List<DictionaryWord>();
            DictionaryWord b1 = new DictionaryWord("barf", true, 0, 0, 0, true, 0, 0);
            DictionaryWord b2 = new DictionaryWord(db.Database.Connection.Database.ToString(), true, 0, 0, 0, true, 0, 0);
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                try
                {
                    db.Database.Connection.Open();
                }
                catch (Exception e)
                {
                    b2 = new DictionaryWord(e.ToString(), true, 0, 0, 0, true, 0, 0);
                }
            }
            DictionaryWord b3 = new DictionaryWord(db.Database.Connection.ConnectionString.ToString(), true, 0, 0, 0, true, 0, 0);
            DictionaryWord b4 = new DictionaryWord(db.Database.Connection.State.ToString(), true, 0, 0, 0, true, 0, 0);
            words.Add(b1);
            words.Add(b2);
            words.Add(b3);
            words.Add(b4);
            */


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

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DictionaryWord dictionaryWord = db.DictionaryWords.Find(id);
            db.DictionaryWords.Remove(dictionaryWord);
            db.SaveChanges();
            return RedirectToAction("Index");
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
