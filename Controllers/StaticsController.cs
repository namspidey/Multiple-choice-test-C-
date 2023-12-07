using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2.Models;

namespace Project2.Controllers
{
    public class StaticsController : Controller
    {
        project_prnContext context = new project_prnContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Overview()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            List<Bailam> list = context.Bailams.Where(x => x.UserId == userId).OrderByDescending(x => x.Date).ToList();
            ViewBag.list = list;

            List<String> listTime = new List<String>();
            List<int?> listIdDe = new List<int?>();
            List<DateTime?> listDate = new List<DateTime?>();
            for (int i = 0; i < list.Count(); i++)
            {
                TimeSpan time = list[i].End.Value - list[i].Start.Value;
                string times = time.Minutes.ToString() + " phút " + time.Seconds.ToString() + " giây";
                listTime.Add(times);
                listIdDe.Add(list[i].DethiId);
                listDate.Add(list[i].Date);
            }
            ViewBag.listDate = listDate;
            ViewBag.time = listTime;
            ViewBag.idDe = listIdDe;
            return View();
        }

        public IActionResult Detail(string? id)
        {
            int bailamId = int.Parse(id);
            Bailam b = context.Bailams.SingleOrDefault(x => x.Id == bailamId);
            Dethi d = context.Dethis.SingleOrDefault(x => x.Id == b.DethiId);
            List<Cauhoi> listH = context.Cauhois.Where(x => x.Id == d.Id).ToList();
            List<TraloiDetail> listC = context.TraloiDetails.Where(x => x.BailamId == bailamId).ToList();

            ViewBag.listC = listC;
            ViewBag.listH = listH;
            var data = from TraloiDetail in context.TraloiDetails
                       join Cauhoi in context.Cauhois
                       on TraloiDetail.QuesId equals Cauhoi.Id
                       where TraloiDetail.BailamId == bailamId
                       select new
                       {
                           nd = Cauhoi.NoiDung,
                           trl = TraloiDetail.Traloi,
                           a = Cauhoi.A,
                           b = Cauhoi.B,
                           c = Cauhoi.C,
                           d = Cauhoi.D,
                           ic = TraloiDetail.IsCorrect,
                           dung = Cauhoi.DapAn
                       };

            ViewBag.data = data.ToList();
            return View(b);
        }

        public IActionResult Retake(string? id)
        {
            int de = int.Parse(id);
            Bailam bailam= context.Bailams.Where(x => x.DethiId == de).OrderByDescending(x => x.Date).FirstOrDefault();
            int BailamId = bailam.Id;
            return RedirectToAction("Detail", new { id = BailamId });
        }
    }
}
