using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forestitan.Models;

namespace Forestitan.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserList()
        {
            return View();
        }
        
        public ActionResult GetUserList()
        {
            using (UsersEntities db = new UsersEntities())
            {
                List<UserAccount> user = db.UserAccounts.ToList<UserAccount>();
                return Json(new { data = user }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}