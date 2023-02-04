using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spbs.Ui.Data.Migrations.Expenses
{
    public partial class ExpenseItemUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ExpenseItems");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Expenses",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ExpenseItems",
                type: "nvarchar(8)",
                nullable: false,
                defaultValue: "");
        }
    }
}
