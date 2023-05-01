using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

public partial class Shift
{
    [Key]
    [Column("shiftID")]
    public short ShiftId { get; set; }

    [Column("workDays")]
    [StringLength(50)]
    public string WorkDays { get; set; } = null!;

    [InverseProperty("Shift")]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
