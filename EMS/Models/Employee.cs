using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    public interface Person
    {
        public abstract void UpdateProfile();
        public abstract void UpdateAttendance();
    }
    public class Employee : Person
    {
        [Required]
        public string Name { get; set; }

        [Required] 
        public string Password { get;set; }

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

        public Project? AssignedProject { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public Employee CreatedBy { get; set; } 

        public Employee LastUpdatedBy { get; set; } 

        public Employee(string name, string password, string email, string phone, BloodGroup bloodGroup, JobPost jobPost, int salary, string? address, Project? assignedProject)
        {
            Name = name;
            Password = password;
            Email = email;
            Phone = phone;
            BloodGroup = bloodGroup;
            JobPost = jobPost;
            Salary = salary;
            Address = address;
            AssignedProject = assignedProject;
        }

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
        public Admin(string name, string password, string email, string phone, BloodGroup bloodGroup, JobPost jobPost, int salary, string? address, Project? assignedProject) : base(name, password, email, phone, bloodGroup, jobPost, salary, address, assignedProject)
        {
        }

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
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative,
        NotSure
    }
    public enum JobPost
    {
        SoftwareEngineer,
        SeniorSoftwareEngineer,
        ProjectLead,
        HR,
        CEO,
        CTO,
        NotDecidedyet
    }
}
