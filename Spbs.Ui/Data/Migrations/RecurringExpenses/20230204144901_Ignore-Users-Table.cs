using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spbs.Ui.Data.Migrations.RecurringExpenses
{
    public partial class IgnoreUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecurringExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OwningUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", nullable: true),
                    BillingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BillingPrincipal = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Total = table.Column<double>(type: "double", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(8)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    BillingType = table.Column<int>(type: "int", nullable: false),
                    RecurrenceType = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringExpenses_User_OwningUserId",
                        column: x => x.OwningUserId,
                        principalTable: "User",
                        principalColumn: "AzureAdId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecurringExpenseHistoryItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Total = table.Column<double>(type: "double", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Payed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RecurringExpenseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringExpenseHistoryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringExpenseHistoryItem_RecurringExpenses_RecurringExpen~",
                        column: x => x.RecurringExpenseId,
                        principalTable: "RecurringExpenses",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringExpenseHistoryItem_RecurringExpenseId",
                table: "RecurringExpenseHistoryItem",
                column: "RecurringExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringExpenses_BillingDate",
                table: "RecurringExpenses",
                column: "BillingDate");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringExpenses_Name",
                table: "RecurringExpenses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringExpenses_OwningUserId",
                table: "RecurringExpenses",
                column: "OwningUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringExpenseHistoryItem");

            migrationBuilder.DropTable(
                name: "RecurringExpenses");
        }
    }
}
