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


        public EmployeeController(ApplicationDbContext dbContext){
            _dbContext = dbContext;
        }

        public IActionResult ShowEmployeeList(){

            var employeeList = _dbContext.Employees
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
        public IActionResult Create([Bind("EmployeeName,EmployeeEmail,JobTitle,Address,Phone,BloodGroup,ProjectId")] Employee employee)
        {
            if (ModelState.IsValid)
            {

                bool isEmailAvailable =  IsEmailAvailable(employee.EmployeeEmail);

                if (!isEmailAvailable)
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                    ViewBag.Projects = _dbContext.Projects.ToList();
                    return View(employee);

                }
                else
                {
                    employee.EmployeeId = Guid.NewGuid();
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
            bool isAvailable = !_dbContext.Employees.Any(e => e.EmployeeEmail == email);
            return isAvailable;
        }

        public IActionResult Edit(Guid id)
        {
            var employee = _dbContext.Employees
                .Include(e => e.Project)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Projects = _dbContext.Projects.ToList();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EmployeeId,EmployeeName,EmployeeEmail,JobTitle,Address,Phone,BloodGroup,ProjectId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(employee);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ShowEmployeeList));
            }

            ViewBag.Projects = _dbContext.Projects.ToList();

            return View(employee);
        }

        public bool EmployeeExists(Guid id)
        {
            var employee = _dbContext.Employees
                .Include(e => e.Project)
                .FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null)
            {
                return false;
            }
            return true;
        }

        public IActionResult Delete(Guid id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeId == id);

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
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(ShowEmployeeList));
        }


    }

}


