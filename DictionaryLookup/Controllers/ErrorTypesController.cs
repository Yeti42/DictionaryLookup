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
    public class ErrorTypesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: ErrorTypes
        public ActionResult Index()
        {
            return View(db.ErrorTypes.ToList());
        }

        // GET: ErrorTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ErrorType errorType = db.ErrorTypes.Find(id);
            if (errorType == null)
            {
                return HttpNotFound();
            }
            return View(errorType);
        }

        // GET: ErrorTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ErrorTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ErrorTypeID,ErrorTypeName")] ErrorType errorType)
        {
            if (ModelState.IsValid)
            {
                db.ErrorTypes.Add(errorType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(errorType);
        }

        // GET: ErrorTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ErrorType errorType = db.ErrorTypes.Find(id);
            if (errorType == null)
            {
                return HttpNotFound();
            }
            return View(errorType);
        }

        // POST: ErrorTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ErrorTypeID,ErrorTypeName")] ErrorType errorType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(errorType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(errorType);
        }

        // GET: ErrorTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ErrorType errorType = db.ErrorTypes.Find(id);
            if (errorType == null)
            {
                return HttpNotFound();
            }
            return View(errorType);
        }

        // POST: ErrorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ErrorType errorType = db.ErrorTypes.Find(id);
            db.ErrorTypes.Remove(errorType);
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
