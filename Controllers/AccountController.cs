using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forestitan.Models;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Forestitan.Controllers
{
    public class AccountController : Controller
    {
        //SqlConnection con = new SqlConnection();
        //SqlCommand com = new SqlCommand();
        //SqlDataReader dr;

        UsersEntities dbmodel = new UsersEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(UserAccount acc)
        {
            using (var dataContext = new UsersEntities())
            {
                var _passWord = PasswordEncryption.textToEncrypt(acc.Password);
                UserAccount user = dataContext.UserAccounts.Where(x => x.Email == acc.Email && x.Password == _passWord).SingleOrDefault();

                if (user==null)
                {
                    ViewBag.ErrorMessage = "Invalid Email or Password";
                    return View("Login", user);
                }
                else
                {
                    Session["userID"] = user.UserID;
                    Session["userName"] = user.UserName;
                    Session["Email"] = user.Email;
                    return RedirectToAction("Welcome", "Account");
                }
            }
            
        }
        public ActionResult LogOut()
        {
            Guid userid = (Guid)Session["userID"];
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount userModel)
        {
            using (UsersEntities dbmodel = new UsersEntities())
            {
                if (dbmodel.UserAccounts.Any(x => x.Email == userModel.Email))
                {
                    return View("SignUpFailed");
                }
                UserAccount user = new UserAccount();
                userModel.UserID = Guid.NewGuid();
                //userModel.DateRegister = DateTime.UtcNow();
                userModel.Password = PasswordEncryption.textToEncrypt(userModel.Password);
                dbmodel.UserAccounts.Add(userModel);
                dbmodel.SaveChanges();
                ModelState.Clear();
                return View("SignUp", user);
            }
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult UserList()
        {
            using (UsersEntities db = new UsersEntities())
            {
                List<UserAccount> user = db.UserAccounts.ToList<UserAccount>();
                return Json(new { data = user }, JsonRequestBehavior.AllowGet);
            }
        }

        //protected ActionResult SignUp(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Guid newGuid = Guid.NewGuid();
        //        SqlConnection conn = new SqlConnection();
        //        conn.Open();
        //        string insertQuery = "insert into UserAccount (UserID, UsernName, Email, Password)";
        //        SqlCommand com = new SqlCommand(insertQuery, con);

        //        com.Parameters.AddWithValue("@UserID", newGuid.ToString());
        //        com.Parameters.AddWithValue("@UserName", );
        //        com.Parameters.AddWithValue("@Email", );
        //        com.Parameters.AddWithValue("@Password", );

        //        com.ExecuteNonQuery();
        //        Response.Redirect("");
        //        Response.Write("successful register");

        //        conn.Close();
        //    }
        //    catch(Exception ex)
        //    {
        //        Response.Write("error"+ex.ToString());
        //    }
        //}



        //connectionString();
        //con.Open();
        //com.Connection = con;
        //com.CommandText = "select Email, Password from UserAccount where Email='" + acc.Email + "' and Password='" + acc.Password + "'";
        //dr = com.ExecuteReader();

        //if (dr.Read())
        //{
        //    con.Close();
        //    return View("SuccesfulLogin");
        //}
        //else
        //{
        //    con.Close();
        //    ViewBag.ErrorMessage = "Invalid Login Credentials";
        //    return View("Error");
        //}


    }
}

