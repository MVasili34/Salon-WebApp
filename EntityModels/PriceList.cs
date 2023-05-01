using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

[Table("PriceList")]
public partial class PriceList
{
    [Key]
    [Column("serviceID")]
    public short ServiceId { get; set; }

    [Column("service")]
    [StringLength(100)]
    [Unicode(false)]
    public string Service { get; set; } = null!;

    [Column("price", TypeName = "smallmoney")]
    public decimal Price { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
