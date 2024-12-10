﻿using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class IncomeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public string Source { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }

}