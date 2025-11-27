using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestX.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRelaship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers",
                column: "WardsCommuneId",
                principalTable: "WardsCommunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Provinces_ProvinceId",
                table: "AspNetUsers",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WardsCommunes_WardsCommuneId",
                table: "AspNetUsers",
                column: "WardsCommuneId",
                principalTable: "WardsCommunes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
