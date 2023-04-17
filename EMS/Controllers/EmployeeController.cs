using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly ApplicationDbContext _dbContext;
        // define readonly logger
        public readonly ILogger<EmployeeController> _Logger;

        public EmployeeController(ApplicationDbContext dbContext, ILogger<EmployeeController> logger){
            _dbContext = dbContext;
            _Logger = logger;
        }

        public IActionResult ShowEmployeeList(){

            var employeeList = _dbContext.Users
                .Include(e => e.Project)
                .Include(e => e.Image).ToList();
            return View(employeeList);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_dbContext.Projects, "ProjectId", "ProjectName");
            ViewBag.Projects = _dbContext.Projects.ToList();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EmployeeName,Email,JobTitle,Address,Phone,BloodGroup,ProjectId")] Employee employee)
        {
            if (ModelState.IsValid)
            {

                bool isEmailAvailable =  IsEmailAvailable(employee.Email);

                if (!isEmailAvailable)
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                    ViewBag.Projects = _dbContext.Projects.ToList();
                    return View(employee);

                }
                else
                {
                    employee.Id = Guid.NewGuid();
                    employee.CreatedAt = DateTime.Now;
                    employee.LastUpdatedAt = DateTime.Now;

                    _dbContext.Add(employee);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(ShowEmployeeList));
                }
            }
            ViewData["ProjectId"] = new SelectList(_dbContext.Projects, "ProjectId", "ProjectName", employee.ProjectId);
            return View(employee);
        }

        public bool IsEmailAvailable(string email)
        {
            bool isAvailable = !_dbContext.Users.Any(e => e.Email == email);
            return isAvailable;
        }

        public IActionResult Edit(Guid id)
        {
            var employee = _dbContext.Users
                .Include(e => e.Project)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Projects = _dbContext.Projects.ToList();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EmployeeName,JobTitle,Address,Phone,BloodGroup,ProjectId")] Employee employee)
        {
            
           
            
            
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
               
                // Find the employee by id
                var employeeToUpdate = _dbContext.Users.FirstOrDefault(e => e.Id == id);
                
                // Update the employee
                employeeToUpdate.EmployeeName = employee.EmployeeName;
                employeeToUpdate.JobTitle = employee.JobTitle;
                employeeToUpdate.Address = employee.Address;
                employeeToUpdate.Phone = employee.Phone;
                employeeToUpdate.BloodGroup = employee.BloodGroup;
                employeeToUpdate.ProjectId = employee.ProjectId;
                employeeToUpdate.LastUpdatedAt = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                
                
                
                return RedirectToAction(nameof(ShowEmployeeList));
            }

            ViewBag.Projects = _dbContext.Projects.ToList();

            return View(employee);
        }

        public bool EmployeeExists(Guid id)
        {
            var employee = _dbContext.Users
                .Include(e => e.Project)
                .FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return false;
            }
            return true;
        }

        public IActionResult Delete(Guid id)
        {
            var employee = _dbContext.Users.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _dbContext.Users.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(ShowEmployeeList));
        }


    }

}


