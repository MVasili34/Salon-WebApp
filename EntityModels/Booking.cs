using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

[Table("Booking")]
public partial class Booking
{
    [Key]
    [Column("bookingID")]
    public long BookingId { get; set; }

    [Column("bookingDate", TypeName = "date")]
    public DateTime BookingDate { get; set; }

    [Column("bookingTime")]
    [Precision(0)]
    public TimeSpan BookingTime { get; set; }

    [Column("clientID")]
    [StringLength(450)]
    public string ClientId { get; set; } = null!;

    [Column("serviceID")]
    public short? ServiceId { get; set; }

    [Column("toPay", TypeName = "smallmoney")]
    public decimal ToPay { get; set; }

    [Column("empleyeeID")]
    [StringLength(50)]
    public string EmpleyeeId { get; set; } = null!;

    [Column("visitMark")]
    public bool VisitMark { get; set; }

    [Column("dateAndTimeOfVisit", TypeName = "smalldatetime")]
    public DateTime? DateAndTimeOfVisit { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("Bookings")]
    public virtual AspNetUser Client { get; set; } = null!;

    [ForeignKey("EmpleyeeId")]
    [InverseProperty("Bookings")]
    public virtual Worker Empleyee { get; set; } = null!;

    [ForeignKey("ServiceId")]
    [InverseProperty("Bookings")]
    public virtual PriceList? Service { get; set; }
}
