using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("Employee")]
public partial class Employee
{
    [Key]
    [Column("employeeID")]
    public int EmployeeId { get; set; }

    [Column("employeeName")]
    [StringLength(100)]
    public string EmployeeName { get; set; } = null!;

    [Column("employeePhone")]
    [StringLength(15)]
    public string EmployeePhone { get; set; } = null!;

    [Column("employeeEmail")]
    [StringLength(50)]
    public string EmployeeEmail { get; set; } = null!;

    [Column("employeeBirthDay")]
    [StringLength(50)]
    public string EmployeeBirthDay { get; set; } = null!;

    [Column("roleID")]
    public int RoleId { get; set; }

    [Column("pass")]
    [StringLength(100)]
    public string Pass { get; set; } = null!;

    [InverseProperty("Employee")]
    public virtual ICollection<CustomerHistory> CustomerHistories { get; set; } = new List<CustomerHistory>();

    [InverseProperty("Employee")]
    public virtual ICollection<EmployeeHistory> EmployeeHistories { get; set; } = new List<EmployeeHistory>();

    [InverseProperty("Employee")]
    public virtual ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    [InverseProperty("Employee")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("Employee")]
    public virtual ICollection<ProductHistory> ProductHistories { get; set; } = new List<ProductHistory>();

    [ForeignKey("RoleId")]
    [InverseProperty("Employees")]
    public virtual Role Role { get; set; } = null!;
}
