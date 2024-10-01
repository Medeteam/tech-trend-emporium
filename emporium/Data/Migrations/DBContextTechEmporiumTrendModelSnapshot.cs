﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DBContextTechEmporiumTrend))]
    partial class DBContextTechEmporiumTrendModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.Cart", b =>
                {
                    b.Property<Guid>("Cart_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Coupon_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Coupon_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("Order")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrdersOrder_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Shopping_status_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Cart_id");

                    b.HasIndex("Coupon_id");

                    b.HasIndex("OrdersOrder_id");

                    b.HasIndex("Shopping_status_id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Category_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category_description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Category_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Category_id");

                    b.ToTable("Categories");

                    b.HasAnnotation("Relational:JsonPropertyName", "categoryObject");
                });

            modelBuilder.Entity("Data.Entities.Coupon", b =>
                {
                    b.Property<Guid>("Coupon_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Coupon_name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Coupon_status")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.HasKey("Coupon_id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Data.Entities.Order", b =>
                {
                    b.Property<Guid>("Order_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Total_price")
                        .HasColumnType("int");

                    b.HasKey("Order_id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Data.Entities.Product", b =>
                {
                    b.Property<Guid>("Product_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Category_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasAnnotation("Relational:JsonPropertyName", "image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasAnnotation("Relational:JsonPropertyName", "title");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "price");

                    b.Property<long>("Stock")
                        .HasColumnType("bigint");

                    b.HasKey("Product_id");

                    b.HasIndex("Category_id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Data.Entities.ProductToCart", b =>
                {
                    b.Property<Guid>("Cart_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Product_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Cart_id", "Product_id");

                    b.HasIndex("Product_id");

                    b.ToTable("ProductsToCart");
                });

            modelBuilder.Entity("Data.Entities.ProductWishList", b =>
                {
                    b.Property<Guid>("Wishlist_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Product_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Wishlist_id", "Product_id");

                    b.HasIndex("Product_id");

                    b.ToTable("ProductWishList");
                });

            modelBuilder.Entity("Data.Entities.Review", b =>
                {
                    b.Property<Guid>("Review_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Product_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Review_content")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Review_name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("User_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Review_id");

                    b.HasIndex("Product_id");

                    b.HasIndex("User_id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Role_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Role_id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Data.Entities.ShoppingStatus", b =>
                {
                    b.Property<Guid>("Shopping_status_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Shopping_status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Shopping_status_id");

                    b.ToTable("ShoppingStatus");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.Property<Guid>("User_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Cart_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Role_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("Wishlist_id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("User_id");

                    b.HasIndex("Cart_id");

                    b.HasIndex("Role_id");

                    b.HasIndex("Wishlist_id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Entities.WishList", b =>
                {
                    b.Property<Guid>("Wishlist_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created_at")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Wishlist_id");

                    b.ToTable("WishList");
                });

            modelBuilder.Entity("Data.Entities.Cart", b =>
                {
                    b.HasOne("Data.Entities.Coupon", "Coupon")
                        .WithMany()
                        .HasForeignKey("Coupon_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Order", "Orders")
                        .WithMany()
                        .HasForeignKey("OrdersOrder_id");

                    b.HasOne("Data.Entities.ShoppingStatus", "ShoppingStatus")
                        .WithMany()
                        .HasForeignKey("Shopping_status_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("Orders");

                    b.Navigation("ShoppingStatus");
                });

            modelBuilder.Entity("Data.Entities.Product", b =>
                {
                    b.HasOne("Data.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("Category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Data.Entities.ProductToCart", b =>
                {
                    b.HasOne("Data.Entities.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("Cart_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("Product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data.Entities.ProductWishList", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany("ProductWishLists")
                        .HasForeignKey("Product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.WishList", "WishList")
                        .WithMany("ProductWishLists")
                        .HasForeignKey("Wishlist_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Data.Entities.Review", b =>
                {
                    b.HasOne("Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("Product_id");

                    b.HasOne("Data.Entities.User", null)
                        .WithMany("Reviews")
                        .HasForeignKey("User_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.HasOne("Data.Entities.Cart", "Cart")
                        .WithMany()
                        .HasForeignKey("Cart_id");

                    b.HasOne("Data.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.WishList", "WishList")
                        .WithMany()
                        .HasForeignKey("Wishlist_id");

                    b.Navigation("Cart");

                    b.Navigation("Role");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Data.Entities.Product", b =>
                {
                    b.Navigation("ProductWishLists");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Data.Entities.WishList", b =>
                {
                    b.Navigation("ProductWishLists");
                });
#pragma warning restore 612, 618
        }
    }
}
