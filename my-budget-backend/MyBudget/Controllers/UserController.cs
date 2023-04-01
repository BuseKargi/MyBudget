using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBudget.Filters;
using MyBudget.Models;

namespace MyBudget.Controllers
{
    public class UserController : Controller
    {
        MyBudgetEntities db = new MyBudgetEntities();
        // GET: User
        [Auth]
        public ActionResult Index()
        {
            var user = db.User.Where(x => x.Id == CurrentSession.User.Id).ToList();
            return View(user);
        }
        [Auth]
        public ActionResult Edit(int? id) 
        {
            if (id!=null)
            {
                var user = db.User.SingleOrDefault(x => x.Id == id);
                if (user!=null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(User model, HttpPostedFileBase LogoPath)
        {
            User data = db.User.SingleOrDefault(x => x.Id == model.Id);
            if (data!=null)
            {
                if (LogoPath!=null)
                {
                    string _LogoPath = $"{model.Fullname}.{LogoPath.ContentType.Split('/')[1]}";
                    LogoPath.SaveAs(Server.MapPath($"~/Assets/images/user/{_LogoPath}"));
                    data.ProfileImage = _LogoPath;
                }
                else
                {
                    model.ProfileImage = null;
                }
                data.Fullname = model.Fullname;
                data.Email = model.Email;
                data.Password = model.Password;
                data.ModifiedDate = model.ModifiedDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            if (id != null)
            {
                User data = db.User.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    db.User.Remove(data);
                    db.SaveChanges();
                    return RedirectToAction("Login","Login");
                }
            }
            return RedirectToAction("Login","Login");
        }
    }
}