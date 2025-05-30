using System;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Data;

public class ExpenseDBContext : IdentityDbContext<User>
    {
        public ExpenseDBContext(DbContextOptions<ExpenseDBContext> options)
            : base(options)
        {
        }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
        .HasIndex(u => u.UserName)
        .IsUnique();

        modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Groceries" },
        new Category { Id = 2, Name = "Leisure" },
        new Category { Id = 3, Name = "Electronics" },
        new Category { Id = 4, Name = "Utilities" },
        new Category { Id = 5, Name = "Clothing" },
        new Category { Id = 6, Name = "Health" },
        new Category { Id = 7, Name = "Others" }
    );
    }
}
