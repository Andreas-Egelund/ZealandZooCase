using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

[PrimaryKey("EventId", "UserId")]
[Table("allEventSignups")]
public partial class AllEventSignup
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }

    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("signup_date", TypeName = "datetime")]
    public DateTime SignupDate { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("AllEventSignups")]
    public virtual AllOurEvent Event { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AllEventSignups")]
    public virtual User User { get; set; } = null!;
}
