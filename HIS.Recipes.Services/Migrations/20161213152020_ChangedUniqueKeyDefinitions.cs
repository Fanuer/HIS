using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class ChangedUniqueKeyDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RecipeTags_Name",
                table: "RecipeTags");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RecipeSteps_RecipeId_Order",
                table: "RecipeSteps");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Ingrediants_Name",
                table: "Ingrediants");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_Name",
                table: "RecipeTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId_Order",
                table: "RecipeSteps",
                columns: new[] { "RecipeId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes",
                columns: new[] { "RecipeId", "SourceId", "Page" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingrediants_Name",
                table: "Ingrediants",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeTags_Name",
                table: "RecipeTags");

            migrationBuilder.DropIndex(
                name: "IX_RecipeSteps_RecipeId_Order",
                table: "RecipeSteps");

            migrationBuilder.DropIndex(
                name: "IX_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Ingrediants_Name",
                table: "Ingrediants");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RecipeTags_Name",
                table: "RecipeTags",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RecipeSteps_RecipeId_Order",
                table: "RecipeSteps",
                columns: new[] { "RecipeId", "Order" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes",
                columns: new[] { "RecipeId", "SourceId", "Page" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Ingrediants_Name",
                table: "Ingrediants",
                column: "Name");
        }
    }
}
