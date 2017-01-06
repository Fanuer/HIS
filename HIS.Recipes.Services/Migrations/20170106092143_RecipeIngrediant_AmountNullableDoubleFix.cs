using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class RecipeIngrediant_AmountNullableDoubleFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "RecipeIngrediants",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "RecipeIngrediants",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
