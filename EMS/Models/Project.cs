using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EMS.Models
{
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }
        [Required]
        [DisplayName(" Project Name")]
        public string ProjectName { get; set; }
        [DisplayName(" Project Description")]
        public string? ProjectDescription { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]  
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy{ get; set; }
        public ICollection<Employee>? Employees { get; set; }


    }

}
