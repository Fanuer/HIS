using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class AddedNoSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes",
                columns: new[] { "RecipeId", "SourceId", "Page" },
                unique: true);
        }
    }
}
