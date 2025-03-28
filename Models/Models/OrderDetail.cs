using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("orderDetailID")]
    public int OrderDetailId { get; set; }

    [Column("oderID")]
    public int OderId { get; set; }

    [Column("productID")]
    public int ProductId { get; set; }

    [Column("orderQuantity")]
    public int OrderQuantity { get; set; }

    [Column("orderPrice")]
    public double? OrderPrice { get; set; }

    [ForeignKey("OderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Oder { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderDetails")]
    public virtual Product Product { get; set; } = null!;
}
