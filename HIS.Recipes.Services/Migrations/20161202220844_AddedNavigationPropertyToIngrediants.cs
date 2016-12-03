using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class AddedNavigationPropertyToIngrediants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId",
                table: "RecipeSourceRecipes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingrediants",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Ingrediants_Name",
                table: "Ingrediants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_RecipeId",
                table: "RecipeSourceRecipes",
                column: "RecipeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeSourceRecipes_RecipeId",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Ingrediants_Name",
                table: "Ingrediants");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ingrediants",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId",
                table: "RecipeSourceRecipes",
                column: "RecipeId");
        }
    }
}
