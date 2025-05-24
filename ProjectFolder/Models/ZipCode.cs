using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

[Index("City", "State", "Country", Name = "UQ_ZipCodes_Location", IsUnique = true)]
public partial class ZipCode
{
    [Key]
    [Column("postalcode")]
    [StringLength(4)]
    [Unicode(false)]
    public string Postalcode { get; set; } = null!;

    [Column("city")]
    [StringLength(50)]
    public string City { get; set; } = null!;

    [Column("state")]
    [StringLength(50)]
    public string State { get; set; } = null!;

    [Column("country")]
    [StringLength(50)]
    public string Country { get; set; } = null!;

    [InverseProperty("AddressPostalcodeNavigation")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
