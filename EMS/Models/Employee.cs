using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{

    public enum BloodGroup
    {
        APositive,
        ANegative,
        BPositive,
        BNegative,
        ABPositive,
        ABNegative,
        OPositive,
        ONegative
    }


    public class Employee
    {


        [Key]
        public Guid EmployeeId { get; set; }
        [Required]
        [DisplayName(" Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [DisplayName(" Email")]
        [Remote(action: "IsEmailAvailable", controller: "Employee", ErrorMessage = "Email already exists.")]
        public string? EmployeeEmail { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public BloodGroup BloodGroup { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }


        public Guid? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
        
        public Guid? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image? Image { get; set; }



    }


   
}
