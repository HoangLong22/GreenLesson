using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreenLesson.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenLesson.Controllers
{
    public class HomeController : Controller
    {
        IHostingEnvironment _env;
        public HomeController(IHostingEnvironment environment)
        {
            _env = environment;
        }
        GreenlessonContext db = new GreenlessonContext();
        public IActionResult Index()
        {
            //ViewBag.KhoaHocMoi = db.Course.OrderByDescending(c => c.Id).Take(4).ToList();
            ViewBag.KhoaHocMoi = db.Course.Include(c => c.User).Include(c => c.Category).Where(C => C.Status == 1).OrderByDescending(C => C.Id).Take(4).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var imagePath = @"\Upload\";
                var uploadPath = _env.WebRootPath + imagePath;

                if(!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var uniqFileName = Guid.NewGuid().ToString();
                var FileName = Path.GetFileName(uniqFileName + "."+file.FileName.Split(".")[1].ToLower());
                string fullPath = uploadPath + FileName;
                imagePath = imagePath + @"\";
                var filePạth = @".." + Path.Combine(imagePath, FileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                ViewData["FileLocation"] = filePạth;
            }
            return View("../Users/Create");
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult lesson()
        {
            GreenlessonContext db = new GreenlessonContext();
            var model = db.Lesson.OrderBy(l => l.Title).ToList();
            return View(model);
        }
    }
}
