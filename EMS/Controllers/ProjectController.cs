using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;

namespace EMS.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly ILogger<ProjectController> _logger;

      

        public IActionResult Index()
        {
            IEnumerable<Project> projectList = _dbContext.Projects;
            return View(projectList);
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = _dbContext.Projects.Include(p => p.Employees).FirstOrDefault(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

            public IActionResult Create()
        {
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {

            if (ModelState.IsValid)
            {
                _dbContext.Projects.Add(project);
                _dbContext.SaveChanges();
                TempData["success"] = "Project Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(project);
            }

        }

        //GET
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = _dbContext.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Project project)
        {

            if (ModelState.IsValid)
            {
                _dbContext.Projects.Update(project);
                _dbContext.SaveChanges();
                TempData["success"] = "Project Edited Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(project);
            }

        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var project = _dbContext.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Guid projectId)
        {

            var project = _dbContext.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }
            _dbContext.Remove(project);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}
