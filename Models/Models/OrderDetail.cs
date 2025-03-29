using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[PrimaryKey("OrderDetailId", "OrderId")]
[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("orderDetailID")]
    public int OrderDetailId { get; set; }

    [Key]
    [Column("orderID")]
    public int OrderId { get; set; }

    [Column("productID")]
    public int ProductId { get; set; }

    [Column("orderQuantity")]
    public int OrderQuantity { get; set; }

    [Column("orderPrice")]
    public double OrderPrice { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
