using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GreenLesson.Models;

namespace GreenLesson.Controllers
{
    public class LessonsController : Controller
    {
        private readonly GreenlessonContext _context;

        public LessonsController(GreenlessonContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            //var GreenlessonContext = _context.Lesson.Include(l => l.Id).Include(l => l.CourseId).Include(l => l.Status).Include(l => l.Title);
            var lessons = await _context.Lesson.Include(x=>x.Course).Include(x=>x.Section).ToListAsync();

            // Convert from LessonModel to LessonViewModel
            var lessonsViewModel = new List<LessonViewModel>();
            foreach (var item in lessons)
            {
                LessonViewModel temp = new LessonViewModel();
                temp.Title = item.Title;
                temp.Contents = item.Contents;
                temp.Status = item.Status;
                temp.CourseName = item.Course.Title;
                temp.SectionName = item.Section.Name;
                lessonsViewModel.Add(temp);
            }

            return View(lessonsViewModel);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.Course)
                .Include(l => l.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
        [HttpPost]
        public IActionResult Create(Post post)
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["SectionId"] = new SelectList(_context.Section, "Id", "Name");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CourseId,SectionId,Contents,Status")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", lesson.CourseId);
            ViewData["SectionId"] = new SelectList(_context.Section, "Id", "Name", lesson.SectionId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", lesson.CourseId);
            ViewData["SectionId"] = new SelectList(_context.Section, "Id", "Name", lesson.SectionId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CourseId,SectionId,Contents,Status")] Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", lesson.CourseId);
            ViewData["SectionId"] = new SelectList(_context.Section, "Id", "Name", lesson.SectionId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.Course)
                .Include(l => l.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lesson.FindAsync(id);
            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(e => e.Id == id);
        }
    }
}
