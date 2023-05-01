using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Salon.DataContext;

[Table("ClientManagment")]
public partial class ClientManagment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string? FullName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Telephone { get; set; }

    public string? Email { get; set; }
}
