using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Entities;

public partial class DemoMicroContext : DbContext
{
    public DemoMicroContext()
    {
    }

    public DemoMicroContext(DbContextOptions<DemoMicroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }


    //---------
    //private string GetConnectionString()
    //{
    //    return Environment.GetEnvironmentVariable("DATABASE_URL");
    //}
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
