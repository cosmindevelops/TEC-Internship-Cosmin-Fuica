﻿// <auto-generated />
using System;
using ApiApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiApp.Migrations
{
    [DbContext(typeof(APIDbContext))]
    [Migration("20240605082034_SeedData")]
    partial class SeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Internship.Model.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Internship.Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SalaryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("SalaryId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Internship.Model.PersonDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonCity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("PersonDetails");
                });

            modelBuilder.Entity("Internship.Model.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PositionId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Internship.Model.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Internship.Model.Salary", b =>
                {
                    b.Property<int>("SalaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.HasKey("SalaryId");

                    b.ToTable("Salaries");
                });

            modelBuilder.Entity("Internship.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Internship.Model.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Internship.Model.Person", b =>
                {
                    b.HasOne("Internship.Model.Position", "Position")
                        .WithMany("Persons")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Internship.Model.Salary", "Salary")
                        .WithMany("Persons")
                        .HasForeignKey("SalaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");

                    b.Navigation("Salary");
                });

            modelBuilder.Entity("Internship.Model.PersonDetails", b =>
                {
                    b.HasOne("Internship.Model.Person", "Person")
                        .WithOne("PersonDetails")
                        .HasForeignKey("Internship.Model.PersonDetails", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Internship.Model.Position", b =>
                {
                    b.HasOne("Internship.Model.Department", "Department")
                        .WithMany("Positions")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Internship.Model.UserRole", b =>
                {
                    b.HasOne("Internship.Model.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Internship.Model.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Internship.Model.Department", b =>
                {
                    b.Navigation("Positions");
                });

            modelBuilder.Entity("Internship.Model.Person", b =>
                {
                    b.Navigation("PersonDetails");
                });

            modelBuilder.Entity("Internship.Model.Position", b =>
                {
                    b.Navigation("Persons");
                });

            modelBuilder.Entity("Internship.Model.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Internship.Model.Salary", b =>
                {
                    b.Navigation("Persons");
                });

            modelBuilder.Entity("Internship.Model.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
