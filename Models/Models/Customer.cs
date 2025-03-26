using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("Customer")]
public partial class Customer
{
    [Key]
    [Column("customerID")]
    public int CustomerId { get; set; }

    [Column("customerName")]
    [StringLength(100)]
    public string CustomerName { get; set; } = null!;

    [Column("customerPhone")]
    [StringLength(15)]
    public string CustomerPhone { get; set; } = null!;

    [Column("customerAddress")]
    [StringLength(100)]
    public string? CustomerAddress { get; set; }

    [Column("customerEmail")]
    [StringLength(50)]
    public string? CustomerEmail { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
