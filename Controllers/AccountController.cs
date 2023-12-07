using Microsoft.AspNetCore.Mvc;

using Project2.Models;

namespace Project1.Controllers
{
    public class Account : Controller
    {
        project_prnContext context = new project_prnContext();

        public IActionResult Index()
        {
            ViewBag.name = HttpContext.Session.GetString("username");
            return View();
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            List<User> listu = context.Users.ToList();
            User u = listu.Find(x => x.Username == username);

            if (u != null && u.Password.Equals(password))
            {

                HttpContext.Session.SetString("username", u.Username.ToString());
                HttpContext.Session.SetInt32("userId", u.Id);
                //ViewBag.name = HttpContext.Session.GetString("username");
                return RedirectToAction("Index");
            }
            else
            {
                string err = "Wrong username/password!";
                ViewBag.err = err;
                return View();
            }

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
