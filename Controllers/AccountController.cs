using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forestitan.Models;
using System.Data.SqlClient;
using Auth0.ManagementApi.Models;
using DAL;
using DAL.Repo;
using EmailSender;

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
        public ActionResult Verify(Models.UserAccount acc)
        {
            using (var dataContext = new UsersEntities())
            {
                var _passWord = PasswordEncryption.textToEncrypt(acc.Password);
                Models.UserAccount user = dataContext.UserAccounts.Where(x => x.Email == acc.Email && x.Password == _passWord).SingleOrDefault();

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
        public ActionResult Register(RegisterModel userModel)
        {
            //using (UsersEntities dbmodel = new UsersEntities())
            //{
            //    if (dbmodel.UserAccounts.Any(x => x.Email == userModel.Email))
            //    {
            //        return View("SignUpFailed");
            //    }
            //    UserAccount user = new UserAccount();
            //    userModel.UserID = Guid.NewGuid();
            //    userModel.DateRegister = DateTime.Now;
            //    userModel.Password = PasswordEncryption.textToEncrypt(userModel.Password);
            //    BuildEmailTemplate(userModel.UserID);
            //    dbmodel.UserAccounts.Add(userModel);
            //    dbmodel.SaveChanges();
            //    ModelState.Clear();
            //    return View("SignUp", user);
            //}
            if (ModelState.IsValid)
            {
                UserRepo user = new UserRepo();

                if (user.CheckEmail(userModel.Email))
                {
                    userModel.Password = PasswordEncryption.textToEncrypt(userModel.Password);
                    user.saveAtLogin(userModel);
                    EmailBuilder.BuildEmailTemplateForNewUser(userModel.UserID);
                    string msg = "An Account Activation Request has been sent to your Email, kindly check your Email`3301`";
                    return RedirectToAction("SignUp", "Account", new { msg });
                }
                else
                {
                    return View("SignUpFailed");
                }
            }
            return View();
        }

        public ActionResult Confirm(Guid id)
        {
            UserRepo obj = new UserRepo();
            ViewBag.regID = id;

            var userInfo = obj.GetUser(id);
            ViewBag.NewUser = userInfo.UserName;
            return View();
        }

        public JsonResult RegisterConfirm(Guid id)
        {
            UserRepo obj = new UserRepo();
            obj.ActivateAccount(id);
            var msg = "Your Email is Verified! Welcome to the Team!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Welcome()
        {
            return View();
        }

        

        //public List<UserAccount> GetUserList()
        //{
        //    using(var context = new UsersEntities())
        //    {
        //        var result = context.UserAccounts.Select(x => new UserAccount() {
        //            UserName=x.UserName,
        //            Email=x.Email
                    
        //        }).ToList();
        //    }
        //}

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

