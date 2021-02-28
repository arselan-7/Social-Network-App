using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using Test.ViewModels;

namespace Test.Controllers
{
    public class AccountController : BaseController
    {
        private Model1Container db = new Model1Container();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind(Prefix = "Item1")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = null;
                    using (Model1Container db = new Model1Container())
                    {
                        user = db.UserSet.FirstOrDefault(u => u.Username == model.UserName && u.Password == model.Password);
                    }

                    if (user != null)
                    {
                        //Store the connected user in session
                        Session[ConnectedUserSession.UserSession] = user;

                        //Then redirect to the Index Action method of Home Controller
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Problem occured... try later");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Prefix = "Item2")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Random random = new Random();
                    int userPicNumber = random.Next(1,9);

                    User newUser = new User {
                        FirstName = model.FirstName,
                        LasName = model.LastName,
                        Username = model.UserName,
                        Password = model.Password,
                        DateOfBirth = model.DateOfBirth,
                        Notes = model.Notes,
                        ImageUrl = string.Format("https://randomuser.me/api/portraits/lego/{0}.jpg", userPicNumber)
                    };

                    db.UserSet.Add(newUser);
                    db.SaveChanges();

                    Session[ConnectedUserSession.UserSession] = newUser;
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "Problem occured... try later");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult LogOut()
        {
            Session[ConnectedUserSession.UserSession] = null;

            return View("LogIn");
        }

        public ActionResult SignIn()
        {
            return View("SignIn");
        }
    }
}