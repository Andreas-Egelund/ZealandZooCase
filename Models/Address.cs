using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZealandZooCase.Models;

public partial class Address
{
    [Key]
    [Column("address_id")]
    public int AddressId { get; set; }

    [Column("street")]
    [StringLength(100)]
    public string Street { get; set; } = null!;

    [Column("address_postalcode")]
    [StringLength(4)]
    [Unicode(false)]
    public string AddressPostalcode { get; set; } = null!;

    [ForeignKey("AddressPostalcode")]
    [InverseProperty("Addresses")]
    public virtual ZipCode AddressPostalcodeNavigation { get; set; } = null!;

    [InverseProperty("Address")]
    public virtual ICollection<AllOurEvent> AllOurEvents { get; set; } = new List<AllOurEvent>();
}
