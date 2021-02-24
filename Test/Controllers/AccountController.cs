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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
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
                        Session["CurrentConnectedUser"] = user;

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

        public ActionResult LogOut()
        {
            Session[ConnectedUserSession.UserSession] = null;

            return View("LogIn");
        }
    }
}