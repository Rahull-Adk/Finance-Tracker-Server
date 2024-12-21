using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class IncomeDTO
    {
        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public string Source { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
