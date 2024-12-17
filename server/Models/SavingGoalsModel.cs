using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class SavingGoalsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal GoalAmount { get; set; }
        [Required]
        public DateTime Deadline { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }


    }
}
