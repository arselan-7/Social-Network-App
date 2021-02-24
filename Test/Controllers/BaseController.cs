using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class BaseController : Controller
    {
        //public static readonly string ConnectedUserSession = "CurrentConnectedUser";

        public User ConnectedUser
        {
            get
            {
                return (User)Session[ConnectedUserSession.UserSession];
            }

            set
            {
                Session[ConnectedUserSession.UserSession] = value;
            }
        }
    }
}