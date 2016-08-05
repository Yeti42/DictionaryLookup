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
    }
}
