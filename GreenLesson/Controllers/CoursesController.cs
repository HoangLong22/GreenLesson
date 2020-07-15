using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GreenLesson.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace GreenLesson.Controllers
{
    public class CoursesController : Controller
    {
        private readonly GreenlessonContext _context;
        private readonly IHostingEnvironment _env;

        public CoursesController(GreenlessonContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Course.Include(c => c.Category).Include(c => c.User).ToListAsync();

            // Convert from CoursesModel to CoursesViewModel
            var coursesViewModels = new List<CoursesViewModel>();
            foreach (var item in courses)
            {
                var temp = new CoursesViewModel();
                temp.Id = item.Id;
                temp.Title = item.Title;
                temp.Description = item.Description;
                temp.Thumbnail = item.Thumbnail;
                temp.CategoryId = item.Category.Name;
                temp.UserId = item.User.Account;
                temp.Status = item.Status;
                coursesViewModels.Add(temp);
            }

            return View(coursesViewModels);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Account");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,UserId,Thumbnail,Status")] CoursesViewModel coursesViewModel)
        {
            if (ModelState.IsValid)
            {
                if (coursesViewModel.Thumbnail != null && coursesViewModel.Thumbnail.Length > 0)
                {
                    var file = coursesViewModel.Thumbnail;
                    var imagePath = @"\Upload\";
                    var uploadPath = _env.WebRootPath + imagePath;

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var uniqFileName = Guid.NewGuid().ToString();
                    var FileName = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".")[1].ToLower());
                    string fullPath = uploadPath + FileName;
                    imagePath += @"\";
                    var filePath = @".." + Path.Combine(imagePath, FileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var courses = new Course();
                    courses.Id = coursesViewModel.Id;
                    courses.Title = coursesViewModel.Title;
                    courses.Description = coursesViewModel.Description;
                    courses.CategoryId = coursesViewModel.CategoryId;
                    courses.UserId = coursesViewModel.UserId;
                    courses.Status = coursesViewModel.Status;
                    courses.Thumbnail = filePath;

                    _context.Add(courses);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", coursesViewModel.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Account", coursesViewModel.UserId);
            return View(coursesViewModel);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", course.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", course.UserId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ShortDescription,Description,CategoryId,UserId,Thumbnail,Status")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", course.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", course.UserId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
