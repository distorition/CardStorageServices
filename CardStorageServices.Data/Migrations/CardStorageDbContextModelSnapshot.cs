﻿// <auto-generated />
using System;
using CardStorageServices.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CardStorageServices.Data.Migrations
{
    [DbContext(typeof(CardStorageDbContext))]
    partial class CardStorageDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CardStorageServices.Data.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordSal")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Passwordhash")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.HasKey("AccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("CardStorageServices.Data.AccountSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionId"), 1L, 1);

                    b.Property<int>("AccountID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("dateTime2");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<string>("SessionToken")
                        .IsRequired()
                        .HasMaxLength(244)
                        .HasColumnType("nvarchar(244)");

                    b.Property<DateTime?>("TimeClosed")
                        .HasColumnType("dateTime2");

                    b.Property<DateTime>("TimeLastRequest")
                        .HasColumnType("dateTime2");

                    b.HasKey("SessionId");

                    b.HasIndex("AccountID");

                    b.ToTable("AccountSession");
                });

            modelBuilder.Entity("CardStorageServices.Data.Card", b =>
                {
                    b.Property<Guid>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CVV")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CardNo")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExData")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CardId");

                    b.HasIndex("ClientId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("CardStorageServices.Data.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"), 1L, 1);

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Patronomic")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SurName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CardStorageServices.Data.AccountSession", b =>
                {
                    b.HasOne("CardStorageServices.Data.Account", "Account")
                        .WithMany("AccountSessions")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("CardStorageServices.Data.Card", b =>
                {
                    b.HasOne("CardStorageServices.Data.Client", "Client")
                        .WithMany("Cards")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("CardStorageServices.Data.Account", b =>
                {
                    b.Navigation("AccountSessions");
                });

            modelBuilder.Entity("CardStorageServices.Data.Client", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
