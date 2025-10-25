using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Resident
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "姓名不可為空")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "房號不可為空")]
        public string RoomNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "管理費不可為負數")]
        public int ManagementFee { get; set; }
    }
}
