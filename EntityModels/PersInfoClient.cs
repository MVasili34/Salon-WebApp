using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

public partial class PersInfoClient
{
    [Key]
    [Column("clientID")]
    public long ClientId { get; set; }

    [Column("fullName")]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Column("dateOfBirth", TypeName = "date")]
    public DateTime DateOfBirth { get; set; }

    [Column("telNumber")]
    [StringLength(20)]
    public string TelNumber { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }
}
