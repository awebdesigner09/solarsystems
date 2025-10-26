using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_QuoteRequests_QuoteRequestId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "QuoteRequestId",
                table: "Orders",
                newName: "QuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_QuoteRequestId",
                table: "Orders",
                newName: "IX_Orders_QuoteId");

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuoteRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Components_NoOfPanels = table.Column<int>(type: "int", nullable: false),
                    Components_NoOfInverters = table.Column<int>(type: "int", nullable: false),
                    Components_NoOfMoutingSystems = table.Column<int>(type: "int", nullable: false),
                    Components_NoOfBatteries = table.Column<int>(type: "int", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_QuoteRequests_QuoteRequestId",
                        column: x => x.QuoteRequestId,
                        principalTable: "QuoteRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_QuoteRequestId",
                table: "Quotes",
                column: "QuoteRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Quotes_QuoteId",
                table: "Orders",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Quotes_QuoteId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.RenameColumn(
                name: "QuoteId",
                table: "Orders",
                newName: "QuoteRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_QuoteId",
                table: "Orders",
                newName: "IX_Orders_QuoteRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_QuoteRequests_QuoteRequestId",
                table: "Orders",
                column: "QuoteRequestId",
                principalTable: "QuoteRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
