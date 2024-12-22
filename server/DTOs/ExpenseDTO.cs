using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class ExpenseDTO
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
