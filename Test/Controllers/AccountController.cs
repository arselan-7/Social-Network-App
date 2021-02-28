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
                        ViewBag.ErrorForm = "LoginError";
                        ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect");
                        return View(new Tuple<LoginViewModel,RegisterViewModel>(model, new RegisterViewModel()));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Un problème est survenu... Veuillez réssayer plus tard");
                    return View(new Tuple<LoginViewModel, RegisterViewModel>(model, new RegisterViewModel()));
                }
            }
            else
            {
                return View(new Tuple<LoginViewModel, RegisterViewModel>(model, new RegisterViewModel()));
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
                catch (Exception)
                {
                    ViewBag.ErrorForm = "RegisterError";
                    ModelState.AddModelError("", "Un problème est survenu... Veuillez réssayer plus tard");
                    return View("LogIn",new Tuple<LoginViewModel, RegisterViewModel>(new LoginViewModel(), model));
                }
            }
            else
            {
                ViewBag.ErrorForm = "RegisterError";
                ModelState.AddModelError("", "Un problème est survenu... Veuillez réssayer plus tard");
                return View("LogIn",new Tuple<LoginViewModel, RegisterViewModel>(new LoginViewModel(), model));
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