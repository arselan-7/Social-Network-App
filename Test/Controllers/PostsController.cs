using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test;
using Test.Filters;
using Test.ViewModels;

namespace Test.Controllers
{
    [CustomAuth]
    public class PostsController : BaseController
    {
        private Model1Container db = new Model1Container();

        // GET: Posts
        public ActionResult Index()
        {
            var postSet = db.PostSet.Include(p => p.User);
            return View(postSet.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.PostSet.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,UserId,CreationDate")] Post post)
        {
            try
            {
                if (!string.IsNullOrEmpty(post.Content))
                {
                    Post newPost = new Post { Content = post.Content, UserId = ConnectedUser.Id, CreationDate = DateTime.Now };

                    db.PostSet.Add(newPost);
                    db.SaveChanges();

                    return Json(new { Success = true });
                }
                
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult AddNewPost(int userId, string newPostContent)
        {
            try
            {
                if (userId == ConnectedUser.Id && !string.IsNullOrEmpty(newPostContent))
                {
                    Post newPost = new Post { Content = newPostContent, UserId = userId, CreationDate = DateTime.Now };

                    db.PostSet.Add(newPost);
                    db.SaveChanges();

                    return Json(new { Success = true });
                }

                return HttpNotFound();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.PostSet.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserSet, "Id", "Username", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int userId, int postId, string newContent)
        {
            try
            {
                if (ConnectedUser.Id == userId && !string.IsNullOrEmpty(newContent))
                {
                    Post post = db.PostSet.Find(postId);
                    if (post == null)
                    {
                        return HttpNotFound();
                    }
                    post.Content = newContent;
                    //db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { Success = true });
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            return HttpNotFound();
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.PostSet.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                //Delete foreign keys of the selected post first
                List<Like> likes = db.LikeSet.Where(l => l.PostId == id).ToList();
                db.LikeSet.RemoveRange(likes);

                //then.. delete the selected post
                Post post = db.PostSet.Find(id);
                db.PostSet.Remove(post);
                db.SaveChanges();

                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        public ActionResult _PostLikes(int userId, string postContent)
        {
            try
            {
                if (userId == ConnectedUser.Id && !string.IsNullOrEmpty(postContent))
                {
                    Post newPost = new Post { Content = postContent, UserId = userId, CreationDate = DateTime.Now };

                    db.PostSet.Add(newPost);

                    db.SaveChanges();

                    var postViewModel = new PostViewModel
                    {
                        Id = newPost.Id,
                        Content = newPost.Content,
                        CreationDate = newPost.CreationDate,
                        UserId = newPost.UserId,
                        User = ConnectedUser,
                        Likes = newPost.Likes
                    };
                    return PartialView(postViewModel);
                }

                return HttpNotFound();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public JsonResult PostLikers(int id)
        {
            try
            {
                var likes = db.PostSet.Where(p => p.Id == id).FirstOrDefault().Likes.ToList();
                var likersList = new List<PostLikersViewModel>();

                foreach (var like in likes)
                {
                    var user = db.UserSet.Find(like.UserId);
                    likersList.Add(new PostLikersViewModel { FullName = string.Format("{0} {1}",user.FirstName, user.LasName), ImageUrl = user.ImageUrl });
                }

                return Json( likersList );
            }
            catch (Exception)
            {
                return Json(null);
            }
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
