using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ReviewAndDbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_User_id",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_User_id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_Cart_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShoppingStatus_Shopping_status_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_User_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Jobs_Job_status_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishList_Wishlist_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsToCart_Products_Product_id",
                table: "ProductsToCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWishList_Products_Product_id",
                table: "ProductWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWishList_WishList_Wishlist_id",
                table: "ProductWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_id",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProductsToCart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Coupon_name",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Review_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Review_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Review_content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Review_id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_Product_id",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Product_id",
                table: "Reviews",
                column: "Product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_User_id",
                table: "Reviews",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_User_id",
                table: "Carts",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories",
                column: "Job_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_User_id",
                table: "Categories",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Carts_Cart_id",
                table: "Orders",
                column: "Cart_id",
                principalTable: "Carts",
                principalColumn: "Cart_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShoppingStatus_Shopping_status_id",
                table: "Orders",
                column: "Shopping_status_id",
                principalTable: "ShoppingStatus",
                principalColumn: "Shopping_status_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_User_id",
                table: "Orders",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products",
                column: "Category_id",
                principalTable: "Categories",
                principalColumn: "Category_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Jobs_Job_status_id",
                table: "Products",
                column: "Job_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WishList_Wishlist_id",
                table: "Products",
                column: "Wishlist_id",
                principalTable: "WishList",
                principalColumn: "Wishlist_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsToCart_Products_Product_id",
                table: "ProductsToCart",
                column: "Product_id",
                principalTable: "Products",
                principalColumn: "Product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWishList_Products_Product_id",
                table: "ProductWishList",
                column: "Product_id",
                principalTable: "Products",
                principalColumn: "Product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWishList_WishList_Wishlist_id",
                table: "ProductWishList",
                column: "Wishlist_id",
                principalTable: "WishList",
                principalColumn: "Wishlist_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_id",
                table: "Users",
                column: "Role_id",
                principalTable: "Roles",
                principalColumn: "Role_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_User_id",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_User_id",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_Cart_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShoppingStatus_Shopping_status_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_User_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Jobs_Job_status_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishList_Wishlist_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsToCart_Products_Product_id",
                table: "ProductsToCart");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWishList_Products_Product_id",
                table: "ProductWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductWishList_WishList_Wishlist_id",
                table: "ProductWishList");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProductsToCart",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Coupon_name",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_User_id",
                table: "Carts",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories",
                column: "Job_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_User_id",
                table: "Categories",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Carts_Cart_id",
                table: "Orders",
                column: "Cart_id",
                principalTable: "Carts",
                principalColumn: "Cart_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShoppingStatus_Shopping_status_id",
                table: "Orders",
                column: "Shopping_status_id",
                principalTable: "ShoppingStatus",
                principalColumn: "Shopping_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_User_id",
                table: "Orders",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products",
                column: "Category_id",
                principalTable: "Categories",
                principalColumn: "Category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Jobs_Job_status_id",
                table: "Products",
                column: "Job_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WishList_Wishlist_id",
                table: "Products",
                column: "Wishlist_id",
                principalTable: "WishList",
                principalColumn: "Wishlist_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsToCart_Products_Product_id",
                table: "ProductsToCart",
                column: "Product_id",
                principalTable: "Products",
                principalColumn: "Product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWishList_Products_Product_id",
                table: "ProductWishList",
                column: "Product_id",
                principalTable: "Products",
                principalColumn: "Product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductWishList_WishList_Wishlist_id",
                table: "ProductWishList",
                column: "Wishlist_id",
                principalTable: "WishList",
                principalColumn: "Wishlist_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_id",
                table: "Users",
                column: "Role_id",
                principalTable: "Roles",
                principalColumn: "Role_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
