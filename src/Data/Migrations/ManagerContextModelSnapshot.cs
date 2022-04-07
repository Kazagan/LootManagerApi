﻿// <auto-generated />
using System;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ManagerContext))]
    partial class ManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.Coin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("InGold")
                        .HasPrecision(10, 4)
                        .HasColumnType("numeric(10,4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("Varchar");

                    b.HasKey("Id");

                    b.ToTable("Coin");
                });

            modelBuilder.Entity("Data.Entities.CoinRoller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CoinId")
                        .HasColumnType("uuid");

                    b.Property<int>("DiceCount")
                        .HasColumnType("integer");

                    b.Property<int>("DiceSides")
                        .HasColumnType("integer");

                    b.Property<int>("Multiplier")
                        .HasColumnType("integer");

                    b.Property<int>("RollMin")
                        .HasColumnType("integer");

                    b.Property<int>("TreasureLevel")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.HasIndex("TreasureLevel", "RollMin");

                    b.ToTable("CoinRoller");
                });

            modelBuilder.Entity("Data.Entities.Good", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CoinId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GoodTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("Varchar");

                    b.HasKey("Id");

                    b.HasIndex("CoinId");

                    b.HasIndex("GoodTypeId");

                    b.ToTable("Good");
                });

            modelBuilder.Entity("Data.Entities.GoodRoller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DiceCount")
                        .HasColumnType("integer");

                    b.Property<int>("DiceSides")
                        .HasColumnType("integer");

                    b.Property<Guid>("GoodId")
                        .HasColumnType("uuid");

                    b.Property<int>("Multiplier")
                        .HasColumnType("integer");

                    b.Property<int>("RollMin")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GoodId");

                    b.ToTable("GoodRoller");
                });

            modelBuilder.Entity("Data.Entities.GoodType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("Varchar");

                    b.HasKey("Id");

                    b.ToTable("GoodType");
                });

            modelBuilder.Entity("Data.Entities.GoodTypeRoller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("GoodTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("RollMin")
                        .HasColumnType("integer");

                    b.Property<int>("TreasureLevel")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GoodTypeId");

                    b.ToTable("GoodTypeRoller");
                });

            modelBuilder.Entity("Data.Entities.CoinRoller", b =>
                {
                    b.HasOne("Data.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");
                });

            modelBuilder.Entity("Data.Entities.Good", b =>
                {
                    b.HasOne("Data.Entities.Coin", "Coin")
                        .WithMany()
                        .HasForeignKey("CoinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.GoodType", "GoodType")
                        .WithMany()
                        .HasForeignKey("GoodTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coin");

                    b.Navigation("GoodType");
                });

            modelBuilder.Entity("Data.Entities.GoodRoller", b =>
                {
                    b.HasOne("Data.Entities.Good", "Good")
                        .WithMany()
                        .HasForeignKey("GoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Good");
                });

            modelBuilder.Entity("Data.Entities.GoodTypeRoller", b =>
                {
                    b.HasOne("Data.Entities.GoodType", "GoodType")
                        .WithMany()
                        .HasForeignKey("GoodTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GoodType");
                });
#pragma warning restore 612, 618
        }
    }
}
