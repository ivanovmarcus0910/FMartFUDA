using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("ProductHistory")]
public partial class ProductHistory
{
    [Key]
    [Column("historyID")]
    public int HistoryId { get; set; }

    [Column("actionType")]
    [StringLength(10)]
    public string ActionType { get; set; } = null!;

    [Column("actionDate")]
    public DateOnly? ActionDate { get; set; }

    [Column("employeeID")]
    public int EmployeeId { get; set; }

    [Column("changeDecription")]
    public string? ChangeDecription { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("ProductHistories")]
    public virtual Employee Employee { get; set; } = null!;
}
