using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StatementAnalyticsDesktop.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    filePath = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(maxLength: 50, nullable: false),
                    PreviousBalance = table.Column<double>(nullable: false),
                    NewBalance = table.Column<double>(nullable: false),
                    StatementDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatementId = table.Column<int>(nullable: false),
                    Bank = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    DateReceivedByUs = table.Column<DateTime>(nullable: false),
                    Details = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Credited = table.Column<bool>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    Contactless = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatementId",
                table: "Transactions",
                column: "StatementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Statements");
        }
    }
}
