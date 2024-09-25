using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class productCategoryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Coupon_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Coupon_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Coupon_status = table.Column<bool>(type: "bit", nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Coupon_id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Job_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Job_status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Job_status_id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingStatus",
                columns: table => new
                {
                    Shopping_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Shopping_status = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingStatus", x => x.Shopping_status_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_id",
                        column: x => x.Role_id,
                        principalTable: "Roles",
                        principalColumn: "Role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Cart_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coupon_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Coupon_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shopping_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Cart_id);
                    table.ForeignKey(
                        name: "FK_Carts_Coupons_Coupon_id",
                        column: x => x.Coupon_id,
                        principalTable: "Coupons",
                        principalColumn: "Coupon_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_ShoppingStatus_Shopping_status_id",
                        column: x => x.Shopping_status_id,
                        principalTable: "ShoppingStatus",
                        principalColumn: "Shopping_status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category_description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Job_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Category_id);
                    table.ForeignKey(
                        name: "FK_Categories_Jobs_Job_status_id",
                        column: x => x.Job_status_id,
                        principalTable: "Jobs",
                        principalColumn: "Job_status_id");
                    table.ForeignKey(
                        name: "FK_Categories_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "WishList",
                columns: table => new
                {
                    Wishlist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => x.Wishlist_id);
                    table.ForeignKey(
                        name: "FK_WishList_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Total_price = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Shopping_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cart_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_Cart_id",
                        column: x => x.Cart_id,
                        principalTable: "Carts",
                        principalColumn: "Cart_id");
                    table.ForeignKey(
                        name: "FK_Orders_ShoppingStatus_Shopping_status_id",
                        column: x => x.Shopping_status_id,
                        principalTable: "ShoppingStatus",
                        principalColumn: "Shopping_status_id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<long>(type: "bigint", nullable: false),
                    Created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Job_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Wishlist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobStatusJob_status_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Product_id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_Category_id",
                        column: x => x.Category_id,
                        principalTable: "Categories",
                        principalColumn: "Category_id");
                    table.ForeignKey(
                        name: "FK_Products_Jobs_JobStatusJob_status_id",
                        column: x => x.JobStatusJob_status_id,
                        principalTable: "Jobs",
                        principalColumn: "Job_status_id");
                    table.ForeignKey(
                        name: "FK_Products_Jobs_Job_status_id",
                        column: x => x.Job_status_id,
                        principalTable: "Jobs",
                        principalColumn: "Job_status_id");
                    table.ForeignKey(
                        name: "FK_Products_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_WishList_Wishlist_id",
                        column: x => x.Wishlist_id,
                        principalTable: "WishList",
                        principalColumn: "Wishlist_id");
                });

            migrationBuilder.CreateTable(
                name: "ProductsToCart",
                columns: table => new
                {
                    Cart_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product_cart_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsToCart", x => new { x.Product_id, x.Cart_id });
                    table.ForeignKey(
                        name: "FK_ProductsToCart_Carts_Cart_id",
                        column: x => x.Cart_id,
                        principalTable: "Carts",
                        principalColumn: "Cart_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsToCart_Products_Product_id",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Product_id");
                });

            migrationBuilder.CreateTable(
                name: "ProductWishList",
                columns: table => new
                {
                    Product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Wishlist_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductWishList_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWishList", x => new { x.Product_id, x.Wishlist_id });
                    table.ForeignKey(
                        name: "FK_ProductWishList_Products_Product_id",
                        column: x => x.Product_id,
                        principalTable: "Products",
                        principalColumn: "Product_id");
                    table.ForeignKey(
                        name: "FK_ProductWishList_WishList_Wishlist_id",
                        column: x => x.Wishlist_id,
                        principalTable: "WishList",
                        principalColumn: "Wishlist_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Coupon_id",
                table: "Carts",
                column: "Coupon_id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_Shopping_status_id",
                table: "Carts",
                column: "Shopping_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_User_id",
                table: "Carts",
                column: "User_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Job_status_id",
                table: "Categories",
                column: "Job_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_User_id",
                table: "Categories",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Cart_id",
                table: "Orders",
                column: "Cart_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Shopping_status_id",
                table: "Orders",
                column: "Shopping_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_id",
                table: "Orders",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category_id",
                table: "Products",
                column: "Category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Job_status_id",
                table: "Products",
                column: "Job_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_JobStatusJob_status_id",
                table: "Products",
                column: "JobStatusJob_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_User_id",
                table: "Products",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Wishlist_id",
                table: "Products",
                column: "Wishlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsToCart_Cart_id",
                table: "ProductsToCart",
                column: "Cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWishList_Wishlist_id",
                table: "ProductWishList",
                column: "Wishlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_id",
                table: "Users",
                column: "Role_id");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_User_id",
                table: "WishList",
                column: "User_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductsToCart");

            migrationBuilder.DropTable(
                name: "ProductWishList");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "ShoppingStatus");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
