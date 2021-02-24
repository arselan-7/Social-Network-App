using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Filters;
using Test.ViewModels;

namespace Test.Controllers
{
    [CustomAuth]
    public class HomeController : BaseController
    {
        private Model1Container db = new Model1Container();
        
        public ActionResult Index()
        {
            var posts = db.PostSet
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    User = p.User,
                    Content = p.Content,
                    IsLike = p.Likes
                .Where(l => l.UserId == ConnectedUser.Id)
                .FirstOrDefault() == null ? false : true,
                    Likes = p.Likes,
                    CreationDate = p.CreationDate
                })
                .OrderByDescending(d => d.CreationDate)
                .ToList();

            return View(posts);
        }

        [HttpPost]
        public ActionResult DislikePost(int id)
        {
            try
            {
                //TODO: change the userId from 1 (default value) to current connected user id.
                var toDelete = db.LikeSet.Where(x => x.UserId == ConnectedUser.Id && x.PostId == id).FirstOrDefault();
                db.LikeSet.Remove(toDelete);
                db.SaveChanges();

                var selectedPost = db.PostSet
                    .Select(u => new PostViewModel
                    {
                        Id = u.Id,
                        User = u.User,
                        Content = u.Content,
                        Likes = u.Likes,
                        CreationDate = u.CreationDate,
                        IsLike = u.Likes
                        .Where(l => l.UserId == ConnectedUser.Id)
                        .FirstOrDefault() == null ? false : true
                    })
                    .Where(f => f.Id == id)
                    .FirstOrDefault();

                return PartialView("_PostLikes", selectedPost);

            }
            catch (Exception ex)
            {
                var selectedPost = db.PostSet
                    .Select(u => new PostViewModel
                    {
                        Id = u.Id,
                        User = u.User,
                        Content = u.Content,
                        Likes = u.Likes,
                        CreationDate = u.CreationDate,
                        IsLike = u.Likes
                        .Where(l => l.UserId == ConnectedUser.Id)
                        .FirstOrDefault() == null ? false : true
                    })
                    .Where(f => f.Id == id)
                    .FirstOrDefault();

                return PartialView("_Post", selectedPost);
            }
        }

        [HttpPost]
        public ActionResult LikePost(int id)
        {
            try
            {
                //TODO: change the userId from 1 (default value) to current connected user id.
                var newLike = new Like();
                newLike.PostId = id;
                newLike.UserId = ConnectedUser.Id;

                db.LikeSet.Add(newLike);
                db.SaveChanges();

                var selectedPost = db.PostSet
                    .Select(u => new PostViewModel
                    {
                        Id = u.Id,
                        User = u.User,
                        Content = u.Content,
                        Likes = u.Likes,
                        CreationDate = u.CreationDate,
                        IsLike = u.Likes
                        .Where(l => l.UserId == ConnectedUser.Id)
                        .FirstOrDefault() == null ? false : true
                    })
                    .Where(f => f.Id == id)
                    .FirstOrDefault();

                return PartialView("_PostLikes", selectedPost);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                var selectedPost = db.PostSet
                    .Select(u => new PostViewModel
                    {
                        Id = u.Id,
                        User = u.User,
                        Content = u.Content,
                        Likes = u.Likes,
                        CreationDate = u.CreationDate,
                        IsLike = u.Likes
                        .Where(l => l.UserId == ConnectedUser.Id)
                        .FirstOrDefault() == null ? false : true
                    })
                    .Where(f => f.Id == id)
                    .FirstOrDefault();

                return PartialView("_Post", selectedPost);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [CustomAuth]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}