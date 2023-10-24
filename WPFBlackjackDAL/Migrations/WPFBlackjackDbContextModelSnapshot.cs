﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WPFBlackjackDAL;

#nullable disable

namespace WPFBlackjackDAL.Migrations
{
    [DbContext(typeof(WPFBlackjackDbContext))]
    partial class WPFBlackjackDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WPFBlackjackEL.Card", b =>
                {
                    b.Property<int>("CardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardID"));

                    b.Property<int?>("HandId")
                        .HasColumnType("int");

                    b.Property<string>("ImageFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ShoeID")
                        .HasColumnType("int");

                    b.Property<int>("Suit")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("CardID");

                    b.HasIndex("HandId");

                    b.HasIndex("ShoeID");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("WPFBlackjackEL.GameState", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GameId"));

                    b.Property<int>("Pot")
                        .HasColumnType("int");

                    b.Property<int>("ShoeID")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.HasIndex("ShoeID");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("WPFBlackjackEL.Hand", b =>
                {
                    b.Property<int>("HandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HandId"));

                    b.Property<bool>("IsBlackJack")
                        .HasColumnType("bit");

                    b.HasKey("HandId");

                    b.ToTable("Hand");
                });

            modelBuilder.Entity("WPFBlackjackEL.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

                    b.Property<int>("Funds")
                        .HasColumnType("int");

                    b.Property<int?>("GameStateGameId")
                        .HasColumnType("int");

                    b.Property<int>("HandId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDealer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWinner")
                        .HasColumnType("bit");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerNumber")
                        .HasColumnType("int");

                    b.Property<int>("PlayerState")
                        .HasColumnType("int");

                    b.HasKey("PlayerId");

                    b.HasIndex("GameStateGameId");

                    b.HasIndex("HandId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("WPFBlackjackEL.Shoe", b =>
                {
                    b.Property<int>("ShoeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoeID"));

                    b.Property<int>("CardsSinceLastShuffle")
                        .HasColumnType("int");

                    b.Property<int>("TotalCards")
                        .HasColumnType("int");

                    b.HasKey("ShoeID");

                    b.ToTable("Shoe");
                });

            modelBuilder.Entity("WPFBlackjackEL.Card", b =>
                {
                    b.HasOne("WPFBlackjackEL.Hand", null)
                        .WithMany("Cards")
                        .HasForeignKey("HandId");

                    b.HasOne("WPFBlackjackEL.Shoe", null)
                        .WithMany("Cards")
                        .HasForeignKey("ShoeID");
                });

            modelBuilder.Entity("WPFBlackjackEL.GameState", b =>
                {
                    b.HasOne("WPFBlackjackEL.Shoe", "Shoe")
                        .WithMany()
                        .HasForeignKey("ShoeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shoe");
                });

            modelBuilder.Entity("WPFBlackjackEL.Player", b =>
                {
                    b.HasOne("WPFBlackjackEL.GameState", null)
                        .WithMany("Players")
                        .HasForeignKey("GameStateGameId");

                    b.HasOne("WPFBlackjackEL.Hand", "Hand")
                        .WithMany()
                        .HasForeignKey("HandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hand");
                });

            modelBuilder.Entity("WPFBlackjackEL.GameState", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("WPFBlackjackEL.Hand", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("WPFBlackjackEL.Shoe", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
