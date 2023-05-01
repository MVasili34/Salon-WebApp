using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

public partial class JobTitle
{
    [Key]
    [Column("positionID")]
    public short PositionId { get; set; }

    [Column("position")]
    [StringLength(100)]
    [Unicode(false)]
    public string Position { get; set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
