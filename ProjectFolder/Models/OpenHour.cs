using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

public partial class OpenHour
{
    [Key]
    [Column("OpenHours_Id")]
    public int OpenHoursId { get; set; }

    [Column("openHours_date", TypeName = "datetime")]
    public DateTime OpenHoursDate { get; set; }

    [Column("openHours_start")]
    public int OpenHoursStart { get; set; }

    [Column("openHours_end")]
    public int OpenHoursEnd { get; set; }
}
