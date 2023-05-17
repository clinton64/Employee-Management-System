using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EMS.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public ProjectController(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache; 
        }

        //private readonly ILogger<ProjectController> _logger;


        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Project> projectList;
            // in-memory caching
            var cacheKey = "Projects";
            if (!_memoryCache.TryGetValue(cacheKey, out projectList))
            {
                projectList = _dbContext.Projects.ToList();
                _memoryCache.Set(cacheKey, projectList, TimeSpan.FromMinutes(5));
            }
            return View(projectList);
        }

        public IActionResult Details(Guid? id)
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
                return View();
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
