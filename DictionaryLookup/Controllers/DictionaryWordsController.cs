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
    public class DictionaryWordsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: DictionaryWords
        public ActionResult Index()
        {
            return View(db.DictionaryWords.ToList());
        }

        // GET: DictionaryWords/Details/5
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

        // GET: DictionaryWords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DictionaryWords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictionaryWordID,Word,Restricted,UnigramProbability,Self")] DictionaryWord dictionaryWord)
        {
            if (ModelState.IsValid)
            {
                db.DictionaryWords.Add(dictionaryWord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictionaryWord);
        }

        // GET: DictionaryWords/Edit/5
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

        // POST: DictionaryWords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictionaryWordID,Word,Restricted,UnigramProbability,Self")] DictionaryWord dictionaryWord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictionaryWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dictionaryWord);
        }

        // GET: DictionaryWords/Delete/5
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

        // POST: DictionaryWords/Delete/5
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
