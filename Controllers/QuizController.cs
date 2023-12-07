using Microsoft.AspNetCore.Mvc;
using Project2.Models;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Project2.Controllers
{
    public class QuizController : Controller
    {

        project_prnContext context = new project_prnContext();
        
        static List<Cauhoi> listRan = new List<Cauhoi>();
        public IActionResult Monthi(string? id)
        {
            
            List<Monhoc> listMon = context.Monhocs.ToList();
            ViewBag.mon = listMon;
            if (id != null)
            {
                int Mid = int.Parse(id);
                List<Dethi> listDe = context.Dethis.Where(item => item.Monhoc.Id == Mid).ToList();
                ViewBag.dethi = listDe;
            }
            
            return View();
        }

        
        public IActionResult Monthi2(string id)
        {
            int Mid = int.Parse(id);
            List<Monhoc> listMon = context.Monhocs.ToList();
            ViewBag.mon = listMon;

            //List<Dethi> listDe = context.Dethis.Where(item => item.Monhoc.Id== Mid).ToList();
            //ViewBag.dethi = listDe;
            return RedirectToAction("Monthi", new { id = Mid });
        }

        public IActionResult LamDe(string? id)
        {
            int Did = int.Parse(id);
            Dethi dethii = context.Dethis.SingleOrDefault(x => x.Id == Did);
            List<Cauhoi> listCau = context.Cauhois.Where(x => x.DethiId == Did).ToList();
            if (dethii.Status == 0)
            {
                Random rd = new Random();
                int n = listCau.Count();
                while (n > 1)
                {
                    n--;
                    int k = rd.Next(n + 1);
                    Cauhoi value = listCau[k];
                    listCau[k] = listCau[n];
                    listCau[n] = value;
                }
                listRan = listCau;
            }
            
            
            dethii.Status = 1;
            context.SaveChanges();

            
            
        
            ViewBag.Cauhoi = listCau;
            ViewBag.DethiId= Did;
            if(listCau.Count == 0)
            {
                ViewBag.mess = "Chưa có thông tin đề";
                
            }
            //bắt đầu làm
            
            DateTime d = DateTime.Now;
            ViewBag.d = d;
            return View();
        }

        public IActionResult Retake(string? id)
        {
            int Did = int.Parse(id);
            Dethi dd = context.Dethis.SingleOrDefault(x => x.Id == Did);
            List<Cauhoi> listCau = listRan;
            Bailam? old = context.Bailams.Where(x=>x.DethiId==Did).OrderByDescending(x=>x.Date).FirstOrDefault();
            List<TraloiDetail> listOld = context.TraloiDetails.Where(x => x.BailamId == old.Id).ToList();

            ViewBag.trls = listOld;
            ViewBag.Cauhoi = listCau;
            ViewBag.DethiId = Did;
            if (listCau.Count == 0)
            {
                ViewBag.mess = "Chưa có thông tin đề";

            }

            var data = from TraloiDetail in context.TraloiDetails
                       join Cauhoi in context.Cauhois
                       on TraloiDetail.QuesId equals Cauhoi.Id
                       where TraloiDetail.BailamId == old.Id
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
            //bắt đầu làm
            ViewBag.data = data.ToList();
            DateTime d = DateTime.Now;
            ViewBag.d = d;

            return View();
        }


        [HttpGet]
        public IActionResult KetQua(string? DethiId,List<string> s)
        {

            int DeId =int.Parse( Request.Query["DethiId"]);
            //int BailamId = int.Parse(Request.Query["BailamId"]);
            List<Cauhoi> listCau = context.Cauhois.Where(x => x.DethiId == DeId).ToList();
            
            Bailam bailam = new Bailam();
            bailam.UserId = HttpContext.Session.GetInt32("userId");
            bailam.DethiId = DeId;
            bailam.Status = 1;
            context.Bailams.Add(bailam);

            context.SaveChanges();
            

            List<TraloiDetail> listDetaill = new List<TraloiDetail>();
            for (int i = 0; i < listCau.Count(); i++)
            {
                listDetaill.Add(new TraloiDetail());
                listDetaill[i].BailamId = bailam.Id;
                listDetaill[i].QuesId = listCau[i].Id;
                listDetaill[i].IsCorrect = 0;
                context.TraloiDetails.Add(listDetaill[i]);
                context.SaveChanges();
            }
            
            List<int> cauTraLoi = new List<int>();
            int stt = 1; 
            for (int i = 0; i < listCau.Count(); i++)
            {
                string fieldName = "cau_" + stt;
                

                if (Request.Query.ContainsKey(fieldName) && int.TryParse(Request.Query[fieldName], out int selectedValue))
                {
                  
                        cauTraLoi.Add(selectedValue);
                    
                }
                stt++;
            }
            int BailamId = bailam.Id;
            ViewBag.l = cauTraLoi;
            ViewBag.bl = BailamId;

            List<TraloiDetail> listDetail = context.TraloiDetails.
                        Where(x => x.BailamId == BailamId).ToList();
            for (int i = 0; i < listDetail.Count(); i++)
            {
                listDetail[i].Traloi = cauTraLoi[i];
                if (listDetail[i].Traloi == listDetail[i].Ques.DapAn)
                {
                    listDetail[i].IsCorrect = 1;
                }
            }
            
            Bailam b = context.Bailams.SingleOrDefault(x=>x.Id==BailamId);
            b.Start = DateTime.Parse(Request.Query["Start"]);
            b.Date= DateTime.Parse(Request.Query["Start"]);
            if (b.End == null)
            {
                b.End = DateTime.Now;
            }
            
            context.SaveChanges();

            float mark = 0;
            float tu =(float) context.TraloiDetails.
                        Where(x => x.BailamId == BailamId && x.IsCorrect == 1)
                        .ToList().Count;
            mark =(float)(tu*10/listCau.Count);
            
            ViewBag.mark = mark;
            ViewBag.dungs = context.TraloiDetails.
                        Where(x => x.BailamId == BailamId && x.IsCorrect == 1)
                        .ToList().Count;
            ViewBag.tong = listCau.Count;
            b.Mark  = mark;

            TimeSpan time = b.End.Value-b.Start.Value;
            string times = time.Minutes.ToString() + " phút " + time.Seconds.ToString() + " giây";
            ViewBag.time = times;
            context.SaveChanges();
            return RedirectToAction("InKetQua",new {id=BailamId});
        }




        public IActionResult InKetQua(int id)
        {
            Bailam b = context.Bailams.SingleOrDefault(x=>x.Id == id);
            Dethi d = context.Dethis.SingleOrDefault(x => x.Id == b.DethiId);
            int numCau = context.Cauhois.Where(x => x.DethiId == d.Id).ToList().Count();
            ViewBag.dungs = context.TraloiDetails.
                        Where(x => x.BailamId == b.Id && x.IsCorrect == 1)
                        .ToList().Count;
            ViewBag.tong =  numCau;
            TimeSpan time = b.End.Value - b.Start.Value;
            string times = time.Minutes.ToString() + " phút " + time.Seconds.ToString() + " giây";
            ViewBag.time = times;
            ViewBag.mark =(float) b.Mark;
            ViewBag.bl = b.Id;

            if (d.Status == 2)
            {
                d.Status = 0;
            }
            context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult ReKetQua(string? DethiId, List<string> s)
        {

            int DeId = int.Parse(Request.Query["DethiId"]);

            Dethi dethii = context.Dethis.SingleOrDefault(x => x.Id == DeId);
            dethii.Status = 2;
            context.SaveChanges();
            //int BailamId = int.Parse(Request.Query["BailamId"]);
            List<Cauhoi> listCau = context.Cauhois.Where(x => x.DethiId == DeId).ToList();

            Bailam bailam = context.Bailams.Where(x => x.DethiId == DeId).OrderByDescending(x => x.Date).FirstOrDefault();
            bailam.Status = 2;
            context.SaveChanges();
            List<TraloiDetail> listDetaill = context.TraloiDetails.Where(x => x.BailamId == bailam.Id).ToList();
            
            List<int> cauTraLoi = new List<int>();
            int stt = 1;
            for (int i = 0; i < listCau.Count(); i++)
            {
                string fieldName = "cau_" + stt;


                if (Request.Query.ContainsKey(fieldName) && int.TryParse(Request.Query[fieldName], out int selectedValue))
                {

                    cauTraLoi.Add(selectedValue);

                }
                stt++;
            }
            int BailamId = bailam.Id;
            ViewBag.l = cauTraLoi;
            ViewBag.bl = BailamId;

            List<TraloiDetail> listDetail = context.TraloiDetails.
                        Where(x => x.BailamId == bailam.Id).ToList();
            for (int i = 0; i < listDetail.Count(); i++)
            {
                listDetail[i].Traloi = cauTraLoi[i];
                if (listDetail[i].Traloi == listDetail[i].Ques.DapAn)
                {
                    listDetail[i].IsCorrect = 1;
                }
            }

            Bailam b = context.Bailams.SingleOrDefault(x => x.Id == BailamId);
            b.Start = DateTime.Parse(Request.Query["Start"]);
            b.Date = DateTime.Parse(Request.Query["Start"]);
            if (b.End == null)
            {
                b.End = DateTime.Now;
            }

            context.SaveChanges();

            float mark = 0;
            float tu = (float)context.TraloiDetails.
                        Where(x => x.BailamId == BailamId && x.IsCorrect == 1)
                        .ToList().Count;
            mark = (float)(tu * 10 / listCau.Count);

            ViewBag.mark = mark;
            ViewBag.dungs = context.TraloiDetails.
                        Where(x => x.BailamId == BailamId && x.IsCorrect == 1)
                        .ToList().Count;
            ViewBag.tong = listCau.Count;
            b.Mark = mark;

            TimeSpan time = b.End.Value - b.Start.Value;
            string times = time.Minutes.ToString() + " phút " + time.Seconds.ToString() + " giây";
            ViewBag.time = times;
            
            context.SaveChanges();
            
            return RedirectToAction("InKetQua", new { id = BailamId });
        }
    }
}
