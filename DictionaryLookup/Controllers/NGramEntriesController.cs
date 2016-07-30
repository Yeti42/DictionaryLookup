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
        
        //Returns the 
        public Int64 ReadOrCreateNGramEntry(string ngram)
        {
            Int64 WordID = 0;
            Int64 Previous1WordID = 0;
            Int64 Previous2WordID = 0;
            string[] words = ngram.Split(' ');
            Int32 NGram = words.Count();

            WordID = ReadOrCreateWord(words[NGram - 1]);
            
            if(NGram > 1)
            {
                Previous1WordID = ReadOrCreateWord(words[NGram - 2]);
            }

            if (NGram > 2)
            {
                Previous2WordID = ReadOrCreateWord(words[NGram - 3]);
            }

            // Made sure all the words are in the WordStrings table now we need to find or create an NGram entry
            var wid = from a in db.NGramEntries
                      where a.WordID.Equals(WordID)
                      where a.Previous1WordID.Equals(Previous1WordID)
                      where a.Previous2WordID.Equals(Previous2WordID)
                      select a.NGramEntryID;
            if (wid.Count() == 0)
            {
                return wid.First();
            }
            NGramEntry nge = new NGramEntry(WordID, Previous1WordID, Previous2WordID);
            db.NGramEntries.Add(nge);
            return nge.NGramEntryID;
        }

        private Int64 ReadOrCreateWord(string word)
        {
            var wid = from a in db.WordStrings
                      where a.Word.Equals(word)
                      select a.WordStringID;
            if (wid.Count() > 0)
            {
                return wid.First();
            }
            WordString ws = new WordString(word);
            db.WordStrings.Add(ws);
            return ws.WordStringID;
        }
        
        // POST: NGramEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NGramEntryID,WordID,Previous1WordID,Previous2WordID,NGram")] NGramEntry nGramEntry)
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
