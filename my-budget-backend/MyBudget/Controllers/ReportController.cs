using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBudget.Models;
using MyBudget.Filters;
namespace MyBudget.Controllers
{
    public class ReportController : Controller
    {
        MyBudgetEntities db = new MyBudgetEntities();

        // GET: Report
        [Auth]
        public ActionResult Index()
        {
            return View();
        }
    }
}