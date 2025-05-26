using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

public partial class OurEvent
{
    [Key]
    [Column("event_id")]
    public int EventId { get; set; }

    [Column("event_name")]
    [StringLength(50)]
    public string EventName { get; set; } = null!;

    [Column("event_description")]
    public string? EventDescription { get; set; }

    [Column("address_id")]
    public int AddressId { get; set; }

    [Column("event_date", TypeName = "datetime")]
    public DateTime EventDate { get; set; }

    [Column("event_maxAttendance")]
    public int? EventMaxAttendance { get; set; }

    [Column("event_ticketPrice", TypeName = "decimal(6, 2)")]
    public decimal EventTicketPrice { get; set; }

    [Column("event_imageName")]
    [StringLength(50)]
    public string? EventImageName { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("AllOurEvents")]
    public virtual Address Address { get; set; } = null!;

    [InverseProperty("Event")]
    public virtual ICollection<AllEventSignup> AllEventSignups { get; set; } = new List<AllEventSignup>();
}
