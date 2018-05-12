using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using username_Wines.DAL;
using username_Wines.Models;

namespace username_Wines.Controllers
{
    public class WinesController : Controller
    {
        private wineEntities db = new wineEntities();

        // GET: Wines
        public ActionResult Index(int? Wine_TypeID)
        {
            PopulateDropDownLists();

            var wines = db.Wines.Include(w => w.WineType);

            //Add filters as required
            if(Wine_TypeID.HasValue)
            {
                wines = wines.Where(w => w.Wine_TypeID == Wine_TypeID);
            }

            return View(wines.ToList());
        }

        // GET: Wines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine wine = db.Wines.Find(id);
            if (wine == null)
            {
                return HttpNotFound();
            }
            return View(wine);
        }

        // GET: Wines/Create
        public ActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Wines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,wineName,wineYear,winePrice,wineHarvest,Wine_TypeID")] Wine wine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Wines.Add(wine);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                {
                    ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have duplicate wine and wine year.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }


            PopulateDropDownLists(wine);
            return View(wine);
        }

        // GET: Wines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine wine = db.Wines.Find(id);
            if (wine == null)
            {
                return HttpNotFound();
            }
            PopulateDropDownLists(wine);
            return View(wine);
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] RowVersion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var wineToUpdate = db.Wines.Find(id);
            if(TryUpdateModel(wineToUpdate,"",
                new string [] { "wineName", "wineYear", "winePrice", "wineHarvest", "Wine_TypeID" }))
            {
                try
                {
                    db.Entry(wineToUpdate).OriginalValues["RowVersion"] = RowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Wine)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Wine was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Wine)databaseEntry.ToObject();
                        if (databaseValues.wineName != clientValues.wineName)
                            ModelState.AddModelError("wineName", "Current value: "
                                + databaseValues.wineName);
                        if (databaseValues.winePrice != clientValues.winePrice)
                            ModelState.AddModelError("winePrice", "Current value: "
                                + databaseValues.winePrice);
                        if (databaseValues.wineYear != clientValues.wineYear)
                            ModelState.AddModelError("wineYear", "Current value: "
                                + databaseValues.wineYear);
                        if (databaseValues.wineHarvest != clientValues.wineHarvest)
                            ModelState.AddModelError("wineHarvest", "Current value: "
                                + String.Format("{0:d}", databaseValues.wineHarvest));
                        if (databaseValues.Wine_TypeID != clientValues.Wine_TypeID)
                            ModelState.AddModelError("Wine_TypeID", "Current value: "
                                + db.Wine_Types.Find(databaseValues.Wine_TypeID).wtName);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        wineToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                    {
                        ModelState.AddModelError("", "Unable to save changes. Remember, you cannot have duplicate wine and wine year.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }

           
            PopulateDropDownLists(wineToUpdate);
            return View(wineToUpdate);
        }

        // GET: Wines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine wine = db.Wines.Find(id);
            if (wine == null)
            {
                return HttpNotFound();
            }
            return View(wine);
        }

        // POST: Wines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wine wine = db.Wines.Find(id);
            try
            {
                db.Wines.Remove(wine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(wine);
        }

        private void PopulateDropDownLists(Wine wine = null)
        {
            var dQuery = from d in db.Wine_Types
                         orderby d.wtName
                         select d;
            ViewBag.Wine_TypeID = new SelectList(dQuery, "ID", "wtname", wine?.Wine_TypeID);
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
