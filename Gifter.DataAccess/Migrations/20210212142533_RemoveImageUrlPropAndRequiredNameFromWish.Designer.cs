﻿// <auto-generated />
using System;
using Gifter.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gifter.DataAccess.Migrations
{
    [DbContext(typeof(GifterDbContext))]
    [Migration("20210212142533_RemoveImageUrlPropAndRequiredNameFromWish")]
    partial class RemoveImageUrlPropAndRequiredNameFromWish
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Gifter.DataAccess.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EventTypes");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.GiftGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("GiftGroups");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.GiftType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GiftType");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WishId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WishId")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Participant", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("GriftGroupId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "GriftGroupId");

                    b.HasIndex("GriftGroupId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WishId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WishId")
                        .IsUnique();

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Auth0Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Auth0Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Auth0Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WishListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WishListId");

                    b.ToTable("Wishes");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("GiftGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GiftGroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Event", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.GiftGroup", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.Event", "Event")
                        .WithMany("GiftGroups")
                        .HasForeignKey("EventId");

                    b.HasOne("Gifter.DataAccess.Models.User", "User")
                        .WithMany("GiftGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Image", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.Wish", "Wish")
                        .WithOne("Image")
                        .HasForeignKey("Gifter.DataAccess.Models.Image", "WishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wish");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Participant", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.GiftGroup", "GiftGroup")
                        .WithMany("Participants")
                        .HasForeignKey("GriftGroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Gifter.DataAccess.Models.User", "User")
                        .WithMany("Participants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("GiftGroup");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Reservation", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Gifter.DataAccess.Models.Wish", "Wish")
                        .WithOne("Reservation")
                        .HasForeignKey("Gifter.DataAccess.Models.Reservation", "WishId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Wish");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Wish", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.WishList", "WishList")
                        .WithMany("Wishes")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.WishList", b =>
                {
                    b.HasOne("Gifter.DataAccess.Models.GiftGroup", "GiftGroup")
                        .WithMany("WishLists")
                        .HasForeignKey("GiftGroupId");

                    b.HasOne("Gifter.DataAccess.Models.User", "User")
                        .WithMany("WishLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GiftGroup");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Event", b =>
                {
                    b.Navigation("GiftGroups");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.GiftGroup", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("WishLists");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.User", b =>
                {
                    b.Navigation("GiftGroups");

                    b.Navigation("Participants");

                    b.Navigation("Reservations");

                    b.Navigation("WishLists");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.Wish", b =>
                {
                    b.Navigation("Image");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("Gifter.DataAccess.Models.WishList", b =>
                {
                    b.Navigation("Wishes");
                });
#pragma warning restore 612, 618
        }
    }
}
