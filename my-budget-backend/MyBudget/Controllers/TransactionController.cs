using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBudget.Filters;
using MyBudget.Models;

namespace MyBudget.Controllers
{
    public class TransactionController : Controller
    {
        MyBudgetEntities db = new MyBudgetEntities();
        // GET: Transaction
        [Auth]
        public ActionResult Index()
        {
            var currentUser = CurrentSession.User.Id;
            var transaction = db.Transaction.Where(x => x.UserId == currentUser).ToList();
            return View(transaction);
        }
        [Auth]
        public ActionResult Create()
        {
            var currentUser = CurrentSession.User.Id;
            ViewBag.AccountId = new SelectList(db.Account.Where(x => x.UserId == currentUser).ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.ParentId == 0 && x.IsActive == true).ToList(), "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Create(Transaction model)
        {
            var currentUser = CurrentSession.User.Id;
            ViewBag.AccountId = new SelectList(db.Transaction.Where(x => x.UserId == currentUser).ToList(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Category.Where(x => x.Id == model.CategoryId && x.IsActive == true).ToList(), "Id", "Name");

            model.AccountId = model.AccountId == null ? null : model.AccountId;
            model.CategoryId = model.CategoryId == null ? null : model.CategoryId;
            model.UserId = currentUser;
            model.CreatedDate = DateTime.Now;
            model.ModifiedDate = DateTime.Now;
            db.Transaction.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            Transaction transaction = db.Transaction.Find(id);

            var categories = db.Category.ToList();
            var accounts = db.Account.ToList();
            var currentCategory= categories.SingleOrDefault(x => x.Id == transaction.CategoryId);
            var currentAccount= accounts.SingleOrDefault(x => x.Id == transaction.AccountId);

            ViewBag.CategoryId = new SelectList(categories, "Id", "Name",currentCategory.Id);
            ViewBag.AccountId = new SelectList(accounts, "Id", "Name",currentAccount.Id);

            return View(transaction);
        }

        [HttpPost]
        public ActionResult Edit(Transaction model)
        {
            Transaction data = db.Transaction.SingleOrDefault(x => x.Id == model.Id);
            data.CategoryId = model.CategoryId;
            data.AccountId = model.AccountId;
            data.Name = model.Name;
            data.Description = model.Description;
            data.Amount = model.Amount;
            data.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (id != null)
            {
                Transaction data = db.Transaction.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    db.Transaction.Remove(data);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}