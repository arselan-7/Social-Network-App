using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test;

namespace Test.Controllers
{
    public class LikesController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Likes
        public ActionResult Index()
        {
            var likeSet = db.LikeSet.Include(l => l.Post).Include(l => l.User);
            return View(likeSet.ToList());
        }

        // GET: Likes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.LikeSet.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // GET: Likes/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.PostSet, "Id", "Content");
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostId,UserId")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.LikeSet.Add(like);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.PostSet, "Id", "Content", like.PostId);
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username", like.UserId);
            return View(like);
        }

        // GET: Likes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.LikeSet.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.PostSet, "Id", "Content", like.PostId);
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username", like.UserId);
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostId,UserId")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.Entry(like).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.PostSet, "Id", "Content", like.PostId);
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username", like.UserId);
            return View(like);
        }

        // GET: Likes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.LikeSet.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Like like = db.LikeSet.Find(id);
            db.LikeSet.Remove(like);
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
