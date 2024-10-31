using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuberBreakfast.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRelationshipToBreakfast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Breakfast",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Breakfast_UserId",
                table: "Breakfast",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Breakfast_User_UserId",
                table: "Breakfast",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breakfast_User_UserId",
                table: "Breakfast");

            migrationBuilder.DropIndex(
                name: "IX_Breakfast_UserId",
                table: "Breakfast");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Breakfast");
        }
    }
}
