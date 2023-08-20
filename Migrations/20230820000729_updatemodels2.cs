using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apitest.Migrations
{
    /// <inheritdoc />
    public partial class updatemodels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_IduserNavigationId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_IduserNavigationId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IduserNavigationId",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IduserNavigationId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IduserNavigationId",
                table: "Accounts",
                column: "IduserNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_IduserNavigationId",
                table: "Accounts",
                column: "IduserNavigationId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
