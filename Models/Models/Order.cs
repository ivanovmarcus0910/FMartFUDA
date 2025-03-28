using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("orderID")]
    public int OrderId { get; set; }

    [Column("customerID")]
    public int CustomerId { get; set; }

    [Column("employeeID")]
    public int EmployeeId { get; set; }

    [Column("orderDate")]
    public DateOnly OrderDate { get; set; }

    [Column("orderAmount")]
    public double OrderAmount { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("Orders")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
