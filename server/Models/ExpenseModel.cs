using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class ExpenseModel
    {
        public int Id { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
