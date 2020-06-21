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
    public class UsersController : Controller
    {
        private readonly GreenlessonContext _context;
        private readonly IHostingEnvironment _env;
        public UsersController(GreenlessonContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var greenlessonContext = _context.Users.Include(u => u.Role);
            return View(await greenlessonContext.ToListAsync());
        }

        //// GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Name");
            return View();
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Thumbnail,Email,Phone,Password,RoleId,Status,Account")] UsersViewModel usersViewModel)
        {
            // sao lai co 2 cai create vay
            // co 1 cai thoi ma a
            // cai nay no tu sinh ra khi e add controller
            if (ModelState.IsValid)
            {
                if (usersViewModel.Thumbnail != null && usersViewModel.Thumbnail.Length > 0)
                {
                    var file = usersViewModel.Thumbnail;
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

                    var users = new Users();
                    users.Id = usersViewModel.Id;
                    users.FirstName = usersViewModel.FirstName;
                    users.LastName = usersViewModel.LastName;
                    users.Email = usersViewModel.Email;
                    users.Phone = usersViewModel.Phone;
                    users.Password = usersViewModel.Password;
                    users.RoleId = usersViewModel.RoleId;
                    users.Status = usersViewModel.Status;
                    users.Account = usersViewModel.Account;
                    users.Thumbnail = filePath;

                    _context.Add(users);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Name", usersViewModel.RoleId);
            return View(usersViewModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Id", users.RoleId);
            return View(users);
        }

         ///POST: Users/Edit/5
         ///To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         ///more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Thumbnail,Email,Phone,Password,RoleId,Status,Account")] Users users)
        {
            if (id != users.Id)
           {
                return NotFound();
           }

            if (ModelState.IsValid)
            {
               try
                {
                   _context.Update(users);
                    await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!UsersExists(users.Id))
                   {
                      }
                  else
                   {
                       throw;
                   }
                }
               return RedirectToAction(nameof(Index));
            }
           ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Id", users.RoleId);
            return View(users);
        }

        //// GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
           if (id == null)
            {
                return NotFound();
            }

           var users = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        //// POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}
