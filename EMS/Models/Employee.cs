using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public interface Person
    {
        public abstract void UpdateProfile();
        public abstract void UpdateAttendance();
    }
    public class Employee : Person
    {
        [Key]
        public Guid EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; }

        [Required]
        public BloodGroup BloodGroup { get; set; }

        [Required]
        public JobPost JobPost { get; set; }

        [Range(0, 1000000)]
        public int Salary { get; set; }

        public string? Address { get; set; }

        [DisplayName("Project")]
        public Project? AssignedProject { get; set; } = null!;

        [ForeignKey("Project")]
        public Guid? ProjectId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public Guid? CreatedBy { get; set; } 

        public Guid? LastUpdatedBy { get; set; } 

        public void UpdateAttendance()
        {
            //attendanceController.UpdateAttendance();
        }

        public void UpdateProfile()
        {   
            // employeeController.UpdateEmployee();
        }
    }

    public class Admin : Employee
    {
        public void AddEmployee(Employee employee)
        {

        }
        public void UpdateEmployee(Employee employee)
        {

        }
        public void DeleteEmployee(Employee employee)
        {

        }
        public void AssignProject(Project project, Employee employee)
        {

        }
    }
    public enum BloodGroup
    {
        Unknown,
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative
    }
    public enum JobPost
    {
        Not_Decided_Yet,
        SoftwareEngineer,
        SeniorSoftwareEngineer,
        ProjectLead,
        HR,
        CEO,
        CTO
    }
}
