using System.ComponentModel.DataAnnotations;
namespace backend.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestId { get; set; }
        public string Description { get; set; } = null!;
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}