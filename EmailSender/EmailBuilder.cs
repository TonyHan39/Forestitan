using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using DAL.Repo;


namespace EmailSender
{
    public class EmailBuilder
    {

        public static void BuildEmailTemplateForNewUser(Guid regID)
        {
            UserRepo objUser = new UserRepo();

            string emailBody = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = objUser.GetUser(regID);
            var url = "https://localhost:44303/" + "Account/Confirm?regID=" + regID;
            emailBody = emailBody.Replace("@ViewBag.NewUserName", regInfo.UserName);
            emailBody = emailBody.Replace("@ViewBag.ConfirmationLink", url);
            emailBody = emailBody.ToString();
            BuildEmailTemplate("Your Account Is Successfully Created", emailBody, regInfo.Email);


            //string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            //var regInfo = dbmodel.UserAccounts.Where(x => x.UserID == regID).FirstOrDefault();
            //var url = "https://localhost:44303/" + "Account/Confirm?regId=" + regID;
            //body = body.Replace("@ViewBag.ConfirmationLink", url);
            //body = body.ToString();
            //BuildEmailTemplate("Your Account Is Successfully Created", body, regInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "tonyhan0309@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc)) { mail.Bcc.Add(new MailAddress(bcc)); }
            if (!string.IsNullOrEmpty(cc)) { mail.CC.Add(new MailAddress(cc)); }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendMail(mail);
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("tonyhan0309@gmail.com", "tony");
            try { client.Send(mail); }
            catch (Exception ex) { throw ex; }
        }
    }
}
