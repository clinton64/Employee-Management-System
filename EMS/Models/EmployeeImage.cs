namespace EMS.Models
{
    public class EmployeeImage
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public byte[] ImageData { get; set; }
    }
}
