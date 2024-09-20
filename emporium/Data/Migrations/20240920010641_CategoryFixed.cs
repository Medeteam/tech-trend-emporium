using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CategoryFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Jobs_JobStatusJob_status_id",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_JobStatusJob_status_id",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "JobStatusJob_status_id",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Job_status_id",
                table: "Categories",
                column: "Job_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories",
                column: "Job_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Jobs_Job_status_id",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Job_status_id",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "JobStatusJob_status_id",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_JobStatusJob_status_id",
                table: "Categories",
                column: "JobStatusJob_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Jobs_JobStatusJob_status_id",
                table: "Categories",
                column: "JobStatusJob_status_id",
                principalTable: "Jobs",
                principalColumn: "Job_status_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
