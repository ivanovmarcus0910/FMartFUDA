using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Models.Models;

public partial class FmartFudaContext : DbContext
{
    public FmartFudaContext()
    {
    }

    public FmartFudaContext(DbContextOptions<FmartFudaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerHistory> CustomerHistories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeHistory> EmployeeHistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderHistory> OrderHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductHistory> ProductHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=FMartFUDA;User Id=sa;Password=@Dmin123;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Customer__19BDBDB3DDBFF2DB");

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.CustomerHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerHistory_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Role");
        });

        modelBuilder.Entity<EmployeeHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Employee__19BDBDB3743E7AB7");

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeHistory_Employee");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Employee");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderPrice).HasDefaultValue(0.0);

            entity.HasOne(d => d.Oder).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<OrderHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__OrderHis__19BDBDB30F459054");

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.OrderHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderHistory_Employee");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Produc).IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__ProductH__19BDBDB3C5730CCA");

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.ProductHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductHistory_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
