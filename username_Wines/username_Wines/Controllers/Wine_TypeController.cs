using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using username_Wines.DAL;
using username_Wines.Models;

namespace username_Wines.Controllers
{
    public class Wine_TypeController : Controller
    {
        private wineEntities db = new wineEntities();

        // GET: Wine_Type
        public ActionResult Index()
        {
            return View(db.Wine_Types.ToList());
        }

        // GET: Wine_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine_Type wine_Type = db.Wine_Types.Find(id);
            if (wine_Type == null)
            {
                return HttpNotFound();
            }
            return View(wine_Type);
        }

        // GET: Wine_Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wine_Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,wtName")] Wine_Type wine_Type)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Wine_Types.Add(wine_Type);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(wine_Type);
        }

        // GET: Wine_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine_Type wine_Type = db.Wine_Types.Find(id);
            if (wine_Type == null)
            {
                return HttpNotFound();
            }
            return View(wine_Type);
        }

        // POST: Wine_Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Wine_Type wine_TypeToUpdate = db.Wine_Types.Find(id);
            if(TryUpdateModel(wine_TypeToUpdate,"",
                new string[] { "wtName" } ))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
           
            return View(wine_TypeToUpdate);
        }

        // GET: Wine_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wine_Type wine_Type = db.Wine_Types.Find(id);
            if (wine_Type == null)
            {
                return HttpNotFound();
            }
            return View(wine_Type);
        }

        // POST: Wine_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wine_Type wine_Type = db.Wine_Types.Find(id);
            try
            {
                db.Wine_Types.Remove(wine_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("FK_"))
                {
                    ModelState.AddModelError("", "You cannot delete a Wine Type that has wines in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(wine_Type);
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
