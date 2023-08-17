using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApi.Migrations
{
    public partial class appUser2MoreColumnAdded_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "appUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "appUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "appUsers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "appUsers");
        }
    }
}
