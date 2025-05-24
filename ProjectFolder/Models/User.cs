using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

[Index("UserEmail", Name = "UQ__Users__B0FBA2127722D182", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_name")]
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [Column("user_email")]
    [StringLength(50)]
    public string UserEmail { get; set; } = null!;

    [Column("user_password")]
    [StringLength(50)]
    public string UserPassword { get; set; } = null!;

    [Column("user_isAdmin")]
    public bool UserIsAdmin { get; set; }

    [Column("user_newsletter")]
    public bool UserNewsletter { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AllEventSignup> AllEventSignups { get; set; } = new List<AllEventSignup>();
}
