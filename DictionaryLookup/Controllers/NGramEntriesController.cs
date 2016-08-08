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
    public class NGramEntriesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: NGramEntries
        public ActionResult Index()
        {
            return View(db.NGramEntries.ToList());
        }

        // GET: NGramEntries/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NGramEntry nGramEntry = db.NGramEntries.Find(id);
            if (nGramEntry == null)
            {
                return HttpNotFound();
            }
            return View(nGramEntry);
        }

        // GET: NGramEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NGramEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NGramEntryID,NGramStringID,NGramTagsID,VersionedDictionaryID")] NGramEntry nGramEntry)
        {
            if (ModelState.IsValid)
            {
                db.NGramEntries.Add(nGramEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nGramEntry);
        }

        // GET: NGramEntries/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NGramEntry nGramEntry = db.NGramEntries.Find(id);
            if (nGramEntry == null)
            {
                return HttpNotFound();
            }
            return View(nGramEntry);
        }

        // POST: NGramEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NGramEntryID,NGramStringID,NGramTagsID,VersionedDictionaryID")] NGramEntry nGramEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nGramEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nGramEntry);
        }

        // GET: NGramEntries/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NGramEntry nGramEntry = db.NGramEntries.Find(id);
            if (nGramEntry == null)
            {
                return HttpNotFound();
            }
            return View(nGramEntry);
        }

        // POST: NGramEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            NGramEntry nGramEntry = db.NGramEntries.Find(id);
            db.NGramEntries.Remove(nGramEntry);
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
