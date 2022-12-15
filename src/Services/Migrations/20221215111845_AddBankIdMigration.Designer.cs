﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Services.Data;

#nullable disable

namespace Services.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20221215111845_AddBankIdMigration")]
    partial class AddBankIdMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Examples.Example", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.ToTable("Examples");
                });

            modelBuilder.Entity("Domain.Inquires.Inquire", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MoneyInSmallestUnit")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfInstallments")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Inquiries");
                });

            modelBuilder.Entity("Domain.Offers.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BankId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("InquireId")
                        .HasColumnType("uuid");

                    b.Property<int>("InterestRateInPromiles")
                        .HasColumnType("integer");

                    b.Property<int>("MoneyInSmallestUnit")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfInstallments")
                        .HasColumnType("integer");

                    b.Property<int>("SourceBank")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Inquires.Inquire", b =>
                {
                    b.OwnsOne("Domain.Users.PersonalData", "PersonalData", b1 =>
                        {
                            b1.Property<Guid>("InquireId")
                                .HasColumnType("uuid");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.Property<string>("GovernmentId")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)");

                            b1.Property<int>("GovernmentIdType")
                                .HasColumnType("integer");

                            b1.Property<int>("JobType")
                                .HasColumnType("integer");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.HasKey("InquireId");

                            b1.ToTable("Inquiries");

                            b1.WithOwner()
                                .HasForeignKey("InquireId");
                        });

                    b.Navigation("PersonalData");
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.OwnsOne("Domain.Users.PersonalData", "PersonalData", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("character varying(250)");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.Property<string>("GovernmentId")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)");

                            b1.Property<int>("GovernmentIdType")
                                .HasColumnType("integer");

                            b1.Property<int>("JobType")
                                .HasColumnType("integer");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("character varying(250)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("PersonalData")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
