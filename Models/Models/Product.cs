using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("productID")]
    public int ProductId { get; set; }

    [Column("productName")]
    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Column("productPrice")]
    public double ProductPrice { get; set; }

    [Column("categoryID")]
    public int CategoryId { get; set; }

    [Column("productImage")]
    [StringLength(255)]
    public string ProductImage { get; set; } = null!;

    [Column("productDecription")]
    [StringLength(255)]
    public string? ProductDecription { get; set; }

    [Column("produc")]
    [StringLength(10)]
    public string? Produc { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
