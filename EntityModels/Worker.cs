using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

public partial class Worker
{
    [Key]
    [Column("patentID")]
    [StringLength(6)]
    public string PatentId { get; set; } = null!;

    [Column("fullName")]
    [StringLength(50)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [Column("telNumber")]
    [StringLength(10)]
    public string TelNumber { get; set; } = null!;

    [Column("shiftID")]
    public short? ShiftId { get; set; }

    [Column("positionID")]
    public short? PositionId { get; set; }

    [InverseProperty("Empleyee")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("PositionId")]
    [InverseProperty("Workers")]
    public virtual JobTitle? Position { get; set; }

    [ForeignKey("ShiftId")]
    [InverseProperty("Workers")]
    public virtual Shift? Shift { get; set; }
}
