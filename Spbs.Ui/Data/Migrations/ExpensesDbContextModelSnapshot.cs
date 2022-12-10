﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spbs.Ui.Data;

#nullable disable

namespace Spbs.Ui.Data.Migrations
{
    [DbContext(typeof(ExpensesDbContext))]
    partial class ExpensesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Spbs.Ui.Features.Expenses.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2048)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("OwningUserId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Recurring")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Venue")
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("OwningUserId");

                    b.HasIndex("Venue");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Spbs.Ui.Features.Expenses.ExpenseItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(2048)");

                    b.Property<Guid?>("ExpenseId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.ToTable("ExpenseItems");
                });

            modelBuilder.Entity("Spbs.Ui.Features.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AzureAdId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Spbs.Ui.Features.Expenses.Expense", b =>
                {
                    b.HasOne("Spbs.Ui.Features.Users.User", null)
                        .WithMany()
                        .HasForeignKey("OwningUserId")
                        .HasPrincipalKey("AzureAdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Spbs.Ui.Features.Expenses.ExpenseItem", b =>
                {
                    b.HasOne("Spbs.Ui.Features.Expenses.Expense", null)
                        .WithMany("Items")
                        .HasForeignKey("ExpenseId");
                });

            modelBuilder.Entity("Spbs.Ui.Features.Expenses.Expense", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
