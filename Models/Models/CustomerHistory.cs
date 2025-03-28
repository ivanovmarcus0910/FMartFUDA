using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("CustomerHistory")]
public partial class CustomerHistory
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
}
