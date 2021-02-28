using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test.Filters;
using Test.ViewModels;

namespace Test.Controllers
{
    [CustomAuth]
    public class UserController : BaseController
    {
        private Model1Container db = new Model1Container();

        // GET: User (Profile)
        public ActionResult Profile(int id)
        {
            try
            {
                User user = db.UserSet.Find(id);

                var connectedUserPosts = db.PostSet
                    .Where(x => x.UserId == id)
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
                    return RedirectToAction("NotFound","Error");
                }
                return View(new Tuple<User, List<PostViewModel>>(user, connectedUserPosts));
            }
            catch (Exception)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }
    }
}