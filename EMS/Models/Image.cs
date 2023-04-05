using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EMS.Models
{
    public class Image
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required]
        public string ImageName { get; set; }
        
        [NotMapped]
        [Required]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]  
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy{ get; set; }
        
        [ForeignKey("Employee")]
        public Guid? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
        


    }

}