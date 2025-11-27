using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestX.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAcccountDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WardsCommunes_Provinces_ProvinceId",
                table: "WardsCommunes");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WardsCommunes_Provinces_ProvinceId",
                table: "WardsCommunes",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WardsCommunes_Provinces_ProvinceId",
                table: "WardsCommunes");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WardsCommunes_Provinces_ProvinceId",
                table: "WardsCommunes",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }
    }
}
