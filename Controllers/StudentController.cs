using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forestitan.Models;
using System.Data.Entity;
using System.Net;

namespace Forestitan.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        StudentEntities dbObj = new StudentEntities();

        public ActionResult StudentList()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using(StudentEntities db =new StudentEntities())
            {
                List<Student> students = db.Students.ToList<Student>();
                return Json(new { data = students }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Student());
            }
            else
            {
                using (StudentEntities db = new StudentEntities())
                {
                    return View(db.Students.Where(x => x.ID == id).FirstOrDefault<Student>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Student student)
        {
            using(StudentEntities db = new StudentEntities())
            {
                if(student.ID == 0)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Added Sucessfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Sucessfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (StudentEntities db = new StudentEntities())
            {
                Student student = db.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                db.Students.Remove(student);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }















        //[HttpPost]
        //public ActionResult AddStudent(Student model)
        //{
        //    using (StudentEntities dbmodel = new StudentEntities())
        //    {
        //        if (dbmodel.Students.Any(x => x.Email == model.Email))
        //        {
        //            ViewBag.Exist = "*Email already exist";
        //            return View("Student");
        //        }
        //        Student obj = new Student();
        //        if (ModelState.IsValid)
        //        {
        //            obj.ID = model.ID;
        //            obj.First_Name = model.First_Name;
        //            obj.Last_Name = model.Last_Name;
        //            obj.Email = model.Email;
        //            obj.PhoneNo = model.PhoneNo;
        //            obj.Course = model.Course;

        //            if (model.ID == 0)
        //            {
        //                dbObj.Students.Add(obj);
        //                dbObj.SaveChanges();
        //            }
        //        }
        //    }
        //    ModelState.Clear();
        //    return View("StudentList", "Student");
        //}

        //public ActionResult Edit(Student model)
        //{
        //    Student obj = new Student();
        //    if (ModelState.IsValid)
        //    {
        //        obj.ID = model.ID;
        //        obj.First_Name = model.First_Name;
        //        obj.Last_Name = model.Last_Name;
        //        obj.Email = model.Email;
        //        obj.PhoneNo = model.PhoneNo;
        //        obj.Course = model.Course;

        //        if (model.ID != 0)
        //        {
        //            dbObj.Entry(obj).State = EntityState.Modified;
        //            dbObj.SaveChanges();
        //        }
        //    }
        //    return View("StudentList");
        //}
        //public ActionResult StudentList()
        //{
        //    var res = dbObj.Students.ToList();
        //    return View(res);
        //}

        //public ActionResult Delete(int id)
        //{
        //    var res = dbObj.Students.Where(x => x.ID == id).First();

        //    dbObj.Students.Remove(res);
        //    dbObj.SaveChanges();

        //    var list = dbObj.Students.ToList();
        //    return View("StudentList", list);
        //}

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student s = dbObj.Students.Find(id);
        //    if (s == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View();
        //}
    }
}