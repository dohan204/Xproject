using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestX.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers",
                column: "WardsCommuneId",
                principalTable: "WardsCommunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers",
                column: "WardsCommuneId",
                principalTable: "WardsCommunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
