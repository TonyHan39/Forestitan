using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class UserRepo : BaseRepo
    {
        public List<UserAccount> GetAllList()
        {
            return (from c in Context.UserAccounts select c).ToList();
        }
        public List<UserDetails> GetList()
        {
            List<UserDetails> objPortals = new List<UserDetails>();

            objPortals = (from u in Context.UserAccounts orderby u.UserName select new { u.UserID, u.UserName, u.Email, u.Password, u.DateRegister, u.IsActive }).ToList().Select(rec => new UserDetails(rec.UserID, rec.UserName, rec.Email, rec.Password, rec.DateRegister, rec.IsActive)).ToList();

            return objPortals;
        }
        public UserDetails GetUserbyID(Guid id)
        {
            UserDetails user = new UserDetails();
            List<UserDetails> objUser = new List<UserDetails>();
            objUser = GetList();
            user = objUser.Where(x => x.UserID == id).FirstOrDefault();
            return user;
        }
        public bool CheckEmail(string _email)
        {
            bool verify = true;
            if (Context.UserAccounts.Any(u => u.Email == _email.Trim())) { verify = false; } else { verify = true; }
            return verify;
        }
        public string GetPassword(string email_pw)
        {
            UserAccount user = (from u in Context.UserAccounts where u.Email == email_pw select u).FirstOrDefault();
            return user.Password;
        }
        public UserAccount GetUser(Guid id)
        {
            UserAccount user = new UserAccount();
            List<UserAccount> userslist = new List<UserAccount>();
            userslist = GetAllList();
            user = userslist.Where(x => x.UserID == id).FirstOrDefault();
            return user;
        }
        public void ActivateAccount(Guid id)
        {
            var getrecord = (from u in Context.UserAccounts where u.UserID == id select u).FirstOrDefault();
            getrecord.IsActive = true;
            Context.SaveChanges();
        }
        public void ChangePassword(Guid id, string pw)
        {
            var getrecord = (from u in Context.UserAccounts where u.UserID == id select u).FirstOrDefault();
            getrecord.Password = pw;
            Context.SaveChanges();
        }
        public void saveAtLogin(RegisterModel save)
        {
            UserAccount user = new UserAccount();
            save.UserID = Guid.NewGuid();
            save.IsActive = false;

            user.UserID = save.UserID;
            user.UserName = save.UserName;
            user.Email = save.Email;
            user.Password = save.Password;
            user.DateRegister = save.DateRegister;
            user.IsActive = save.IsActive;
            Context.UserAccounts.Add(user);
            Context.SaveChanges();
        }
        public void Save(UserAccount user)
        {
            user.UserID = Guid.NewGuid();
            user.IsActive = true;
            Context.UserAccounts.Add(user);
            Context.SaveChanges();
        }
        public void Update(Guid id, UserAccount update)
        {
            UserAccount user = (from c in Context.UserAccounts where c.UserID == id select c).FirstOrDefault();
            user.UserName = update.UserName;
            user.Email = update.Email;
            user.Password = update.Password;
            Context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            UserAccount delete = (from c in Context.UserAccounts where c.UserID == id select c).FirstOrDefault();
            delete.IsActive = false;
            Context.SaveChanges();
        }
        public void Recover(Guid id)
        {
            UserAccount delete = (from c in Context.UserAccounts where c.UserID == id select c).FirstOrDefault();
            delete.IsActive = true;
            Context.SaveChanges();
        }
        public UserAccount GetCredentials(string email)
        {
            UserAccount objUser = new UserAccount();
            if (Context.UserAccounts.Any(u => u.Email == email.Trim() && u.IsActive == true))
            {
                objUser = Context.UserAccounts.SingleOrDefault(rec => rec.Email == email.Trim());
            }
            else { objUser = null; }
            return objUser;
        }
    }
    public class UserDetails
    {
        public UserDetails()
        {
        }

        public UserDetails(Guid _id, string _name, string _email, string _password, DateTime? _dateRegister, bool? _isActive)
        {
            UserID = _id;
            UserName = _name;
            Email = _email;
            Password = _password;
            DateRegister = _dateRegister;
            IsActive = _isActive;
        }


        public System.Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> DateRegister { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
