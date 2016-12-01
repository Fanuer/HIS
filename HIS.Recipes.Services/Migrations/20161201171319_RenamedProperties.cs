using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class RenamedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngrediants_Recipes_MemberId",
                table: "RecipeIngrediants");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "RecipeIngrediants",
                newName: "RecipeId1");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngrediants_MemberId",
                table: "RecipeIngrediants",
                newName: "IX_RecipeIngrediants_RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngrediants_Recipes_RecipeId1",
                table: "RecipeIngrediants",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngrediants_Recipes_RecipeId1",
                table: "RecipeIngrediants");

            migrationBuilder.RenameColumn(
                name: "RecipeId1",
                table: "RecipeIngrediants",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngrediants_RecipeId1",
                table: "RecipeIngrediants",
                newName: "IX_RecipeIngrediants_MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngrediants_Recipes_MemberId",
                table: "RecipeIngrediants",
                column: "MemberId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
