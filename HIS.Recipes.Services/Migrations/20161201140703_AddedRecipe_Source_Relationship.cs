using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class AddedRecipe_Source_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeBaseSources_SourceId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeSourceRecipes_Recipes_RecipeId1",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeSourceRecipes_RecipeId1",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SourceId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeSourceRecipes");

            migrationBuilder.AlterColumn<int>(
                name: "Page",
                table: "RecipeSourceRecipes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes",
                columns: new[] { "RecipeId", "SourceId", "Page" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSourceRecipes_Recipes_RecipeId",
                table: "RecipeSourceRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeSourceRecipes_Recipes_RecipeId",
                table: "RecipeSourceRecipes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RecipeSourceRecipes_RecipeId_SourceId_Page",
                table: "RecipeSourceRecipes");

            migrationBuilder.AlterColumn<int>(
                name: "Page",
                table: "RecipeSourceRecipes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<Guid>(
                name: "RecipeId1",
                table: "RecipeSourceRecipes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_RecipeId1",
                table: "RecipeSourceRecipes",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SourceId",
                table: "Recipes",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeBaseSources_SourceId",
                table: "Recipes",
                column: "SourceId",
                principalTable: "RecipeBaseSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSourceRecipes_Recipes_RecipeId1",
                table: "RecipeSourceRecipes",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
