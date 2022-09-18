using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardStorageServices.Data.Migrations
{
    public partial class AccountsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false),
                    PasswordSal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Passwordhash = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "AccountSession",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionToken = table.Column<string>(type: "nvarchar(244)", maxLength: 244, nullable: false),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "dateTime2", nullable: false),
                    TimeLastRequest = table.Column<DateTime>(type: "dateTime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    TimeClosed = table.Column<DateTime>(type: "dateTime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSession", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_AccountSession_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountSession_AccountID",
                table: "AccountSession",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSession");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
