using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Resident
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string RoomNumber { get; set; } = string.Empty;

        public int ManagementFee { get; set; } = 0;
    }
}
