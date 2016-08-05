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
    public class NGramStringsController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: NGramStrings
        public ActionResult Index()
        {
            return View(db.NGramStrings.ToList());
        }

        public string getNGramString(Int64 nGramStringID)
        {
            var nGramStringWords = from ngs1 in db.NGramStrings
                                   where ngs1.NGramStringID == nGramStringID
                                   join wd1 in db.WordStrings on ngs1.WordID equals wd1.WordStringID
                                   join pw1 in db.WordStrings on ngs1.Previous1WordID equals pw1.WordStringID
                                   join pw2 in db.WordStrings on ngs1.Previous2WordID equals pw2.WordStringID
                                   select new { w1 = wd1.Word, p1 = pw1.Word, p2 = pw2.Word };

            return (nGramStringWords.First().p2 + " " + nGramStringWords.First().p1 + " " + nGramStringWords.First().w1).TrimStart(' ');
        }
    }
}
