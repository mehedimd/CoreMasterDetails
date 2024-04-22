using CoreMasterDetails.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace CoreMasterDetails.Controllers
{
    public class FacultyController : Controller
    {
        private readonly FacultyStudentContext context;
        IHostEnvironment environment;
        public FacultyController(FacultyStudentContext context, IHostEnvironment env = null)
        {
            this.context = context;
            this.environment = env;
        }
        public IActionResult Index()
        {
            return View(context.Faculties.Include(a => a.Students).ToList());
        }
        public ActionResult Create()
        {
            Faculty faculty = new Faculty();
            faculty.Students.Add(new Student()
            {
                StudentName = "",
                Address = ""
            });
            return View(faculty);
        }
        [HttpPost]
        public IActionResult Create(Faculty faculty, string btn)
        {
            if (btn == "ADD")
            {
                faculty.Students.Add(new Student());
            }
            if (btn == "Create")
            {
                if (ModelState.IsValid)
                {
                    if (faculty.Picture != null)
                    {
                        // var ext = Path.GetExtension(faculty.Picture.FileName);
                        var rootPath = this.environment.ContentRootPath;
                        var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", faculty.Picture.FileName);
                        using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                        {
                            faculty.Picture.CopyToAsync(fileStream);
                        }
                        faculty.PicPath = "~/Pictures/" + faculty.Picture.FileName;

                        context.Faculties.Add(faculty);
                        if (context.SaveChanges() > 0)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Provide Profile Picture");
                        return View(faculty);
                    }
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
                    ModelState.AddModelError("", message);
                }
            }
            return View(faculty);
        }
        public IActionResult Edit(int id)
        {

            return View(context.Faculties.Include(f => f.Students).Where(f => f.ID.Equals(id)).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult Edit(Faculty faculty, string btn)
        {
            if (btn == "ADD")
            {
                faculty.Students.Add(new Student());
            }
            if (btn == "Create")
            {
                //if (ModelState.IsValid)
                //{
                    var oldFaculty = context.Faculties.Find(faculty.ID);
                    if (faculty.Picture != null)
                    {
                        // var ext = Path.GetExtension(faculty.Picture.FileName);
                        var rootPath = this.environment.ContentRootPath;
                        var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", faculty.Picture.FileName);
                        using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                        {
                            faculty.Picture.CopyToAsync(fileStream);
                        }
                        faculty.PicPath = "~/Pictures/" + faculty.Picture.FileName;

                    }
                    else
                    {
                        oldFaculty.PicPath = oldFaculty.PicPath;
                    }
                    oldFaculty.FacultyName = faculty.FacultyName;
                    oldFaculty.CourseName = faculty.CourseName;
                    oldFaculty.CourseStartDate = faculty.CourseStartDate;
                    context.Students.RemoveRange(context.Students.Where(s => s.FacultyID == faculty.ID));
                    context.SaveChanges();
                    oldFaculty.Students = faculty.Students;
                    context.Entry(oldFaculty).State = EntityState.Modified;
                    if (context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                //}
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
                    ModelState.AddModelError("", message);
                }
            }
            return View(faculty);
        }
        public IActionResult Delete(int id)
        {
            context.Faculties.Remove(context.Faculties.Find(id));
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
