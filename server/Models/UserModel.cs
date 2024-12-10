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
        [StringLength(20, ErrorMessage = "Password should not be more than 20 characters but at least 6 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        public ICollection<IncomeModel> Incomes { get; set; }
        public ICollection<ExpenseModel> Expenses { get; set; }


    }

}
