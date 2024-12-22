using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(20, MinimumLength =4, ErrorMessage = "Username must be between 4 and 20 characters")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        
        public string Password { get; set; }

        public decimal Balance { get; set; }

        public string? AvatarUrl { get; set; }

        public ICollection<IncomeModel> Incomes { get; set; } = new List<IncomeModel>();
        public ICollection<ExpenseModel> Expenses { get; set; } = new List<ExpenseModel>();
        public ICollection<BudgetModel> Budgets { get; set; } = new List<BudgetModel>();


    }

}
