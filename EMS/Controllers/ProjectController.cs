using EMS.Data;
using EMS.Models;
using EMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public ProjectController(ApplicationDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        //private readonly ILogger<ProjectController> _logger;


        [AllowAnonymous]
        public IActionResult Index()
        {
            var cacheKey = "projects";
            var cacheData = _cacheService.GetData<IEnumerable<Project>>(cacheKey);
            IEnumerable<Project> projectList;
            if (cacheData != null && cacheData.Count() > 0)
            {
               projectList = cacheData.ToList();
            }
            else
            {
                projectList = _dbContext.Projects;
                _cacheService.SetData<IEnumerable<Project>>(cacheKey, projectList, DateTimeOffset.Now.AddSeconds(20));
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
