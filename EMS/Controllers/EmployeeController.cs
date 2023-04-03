using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployeeController (ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> Employees = _dbContext.Employees;
            return View(Employees);
        }

        public IActionResult Create()
        {
            // pass blood Group as SelectList
            var groups = Enum.GetValues(typeof(BloodGroup))
                .Cast<BloodGroup>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.BloodGroup = groups;

            // Pass Job Designation as SelectList
            var posts = Enum.GetValues(typeof(JobPost))
                .Cast<JobPost>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.JobPost = posts;

            // pass Project List as SelectList
            var projects = _dbContext.Projects.Select(c => new SelectListItem
            {
                Value = c.ProjectId.ToString(),
                Text = c.ProjectName
            });
            var projectList = new SelectList(projects, "Value", "Text");
            ViewBag.Projects = projectList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _dbContext.Employees.Find(id); //Include(e => e.AssignedProject).FirstOrDefault(e => e.ProjectId == id); ;
            if (employee == null)
            {
                return NotFound();
            }
            employee.AssignedProject = _dbContext.Projects.Find(employee.ProjectId);

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            var groups = Enum.GetValues(typeof(BloodGroup))
                .Cast<BloodGroup>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.BloodGroup = groups;

            var posts = Enum.GetValues(typeof(JobPost))
                .Cast<JobPost>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.JobPost = posts;

            var projects = _dbContext.Projects
                .Select(c => new SelectListItem {Value = c.ProjectId.ToString(), Text = c.ProjectName });
            ViewBag.Projects = projects;

            if (id == null)
            {
                return NotFound();
            } 

            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            // Pass Blood Group as SelectList
            var groups = Enum.GetValues(typeof(BloodGroup))
                .Cast<BloodGroup>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.BloodGroup = groups;

            // Pass Job Designation as SelectList
            var posts = Enum.GetValues(typeof(JobPost))
                .Cast<JobPost>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() })
                .ToList();
            ViewBag.JobPost = posts;

            // pass Project List as SelectList
            var projects = _dbContext.Projects.Select(c => new SelectListItem
            {
                Value = c.ProjectId.ToString(),
                Text = c.ProjectName
            });
            var projectList = new SelectList(projects, "Value", "Text");
            ViewBag.Projects = projectList;

            if (ModelState.IsValid)
            {
                _dbContext.Employees.Update(employee);
                _dbContext.SaveChanges();
                TempData["success"] = "Project Edited Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {

            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            _dbContext.Remove(employee);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
