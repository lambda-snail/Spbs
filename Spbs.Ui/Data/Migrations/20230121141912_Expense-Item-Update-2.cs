using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spbs.Ui.Data.Migrations
{
    public partial class ExpenseItemUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId1",
                table: "ExpenseItems");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_ExpenseId1",
                table: "ExpenseItems");

            migrationBuilder.DropColumn(
                name: "ExpenseId1",
                table: "ExpenseItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExpenseId",
                table: "ExpenseItems",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                table: "ExpenseItems",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                table: "ExpenseItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExpenseId",
                table: "ExpenseItems",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId1",
                table: "ExpenseItems",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_ExpenseId1",
                table: "ExpenseItems",
                column: "ExpenseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId",
                table: "ExpenseItems",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_Expenses_ExpenseId1",
                table: "ExpenseItems",
                column: "ExpenseId1",
                principalTable: "Expenses",
                principalColumn: "Id");
        }
    }
}
