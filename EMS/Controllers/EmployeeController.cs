using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;

namespace EMS.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployeeController (ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Employee> Employees = _dbContext.Employees.Include(e => e.Project);
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

        public IActionResult Details(string? id)
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
            // load Project and Image into employee model
            employee.Project = _dbContext.Projects.Find(employee.ProjectId);
            if(employee.ImageId != null)
            {
                employee.Image = _dbContext.EmployeeImages.Find(employee.ImageId).ImageData;
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(string? id)
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

            // authorize an employee to edit own profile only
            var employee = _dbContext.Employees.Find(id);
            var currentUser = User.Identity.Name;
            if (employee != null && currentUser != null && currentUser == employee.UserName) 
            {
                return View(employee);
            }
            else
            {
                ViewBag.AlertMessage = "You are only authorized to edit your own profile";
                ViewBag.Id = id.ToString();
                return RedirectToAction("Index", "Employee");
            } 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee, IFormFile image)
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
                Employee updatedEmployee = _dbContext.Employees.Find(employee.Id);
                if (updatedEmployee != null)
                {
                    updatedEmployee.UserName = employee.UserName;
                    updatedEmployee.Email = employee.Email;
                    updatedEmployee.PhoneNumber = employee.PhoneNumber;
                    updatedEmployee.Address = employee.Address;
                    updatedEmployee.BloodGroup = employee.BloodGroup;
                    updatedEmployee.Salary = employee.Salary;
                    updatedEmployee.JobPost = employee.JobPost;
                    updatedEmployee.ProjectId = employee.ProjectId;
                    // save image to database 
                    if (image != null && image.Length > 0)
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            var imageData = memoryStream.ToArray();
                            var employeeImage = new EmployeeImage
                            {
                                EmployeeId = updatedEmployee.Id,
                                ImageData = imageData
                            };
                            _dbContext.EmployeeImages.Add(employeeImage);
                            await _dbContext.SaveChangesAsync();
                            updatedEmployee.ImageId = employeeImage.Id;
                        }
                    }
                    _dbContext.Employees.Update(updatedEmployee);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(string? id)
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
        public IActionResult DeleteEmployee(string id)
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
        public bool UploadImage(Employee employee, IFormFile imageFile)
        {
            if(employee != null && imageFile != null && imageFile.Length > 0) 
            {
                using(var stream = new MemoryStream())
                {
                    imageFile.CopyToAsync(stream);
                    employee.Image = stream.ToArray();
                }
                _dbContext.EmployeeImages.Add((EmployeeImage)imageFile);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
