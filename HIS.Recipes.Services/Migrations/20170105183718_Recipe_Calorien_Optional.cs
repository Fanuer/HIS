using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class Recipe_Calorien_Optional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Calories",
                table: "Recipes",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Calories",
                table: "Recipes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
