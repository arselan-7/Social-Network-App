using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test.ViewModels;

namespace Test.Controllers
{
    public class UserController : BaseController
    {
        private Model1Container db = new Model1Container();

        // GET: User (Profile)
        public ActionResult Profile(int id)
        {
            User user = db.UserSet.Find(id);

            var connectedUserPosts = db.PostSet
                .Where(x => x.UserId == ConnectedUser.Id)
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

            Console.WriteLine(connectedUserPosts);



            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new Tuple<User,List<PostViewModel>>(user,connectedUserPosts));
        }
    }
}