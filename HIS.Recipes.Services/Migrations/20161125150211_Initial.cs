﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HIS.Recipes.Services.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingrediants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingrediants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeBaseSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ISBN = table.Column<string>(nullable: true),
                    PublishingCompany = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeBaseSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTags", x => x.Id);
                    table.UniqueConstraint("AK_RecipeTags_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Calories = table.Column<int>(nullable: false),
                    CookedCounter = table.Column<int>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    LastTimeCooked = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NumberOfServings = table.Column<int>(nullable: false),
                    SourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_RecipeBaseSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "RecipeBaseSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Filename = table.Column<string>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeImages_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngrediants",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    IngrediantId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    CookingUnit = table.Column<int>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngrediants", x => new { x.RecipeId, x.IngrediantId });
                    table.UniqueConstraint("AK_RecipeIngrediants_RecipeId", x => x.RecipeId);
                    table.UniqueConstraint("AK_RecipeIngrediants_IngrediantId_RecipeId", x => new { x.IngrediantId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeIngrediants_Ingrediants_IngrediantId",
                        column: x => x.IngrediantId,
                        principalTable: "Ingrediants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngrediants_Recipes_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeRecipeTag",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    RecipeTagId = table.Column<Guid>(nullable: false),
                    RecipeId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeRecipeTag", x => new { x.RecipeId, x.RecipeTagId });
                    table.UniqueConstraint("AK_RecipeRecipeTag_RecipeId", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_RecipeRecipeTag_Recipes_RecipeId1",
                        column: x => x.RecipeId1,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeRecipeTag_RecipeTags_RecipeTagId",
                        column: x => x.RecipeTagId,
                        principalTable: "RecipeTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSourceRecipes",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(nullable: false),
                    SourceId = table.Column<Guid>(nullable: false),
                    Page = table.Column<int>(nullable: true),
                    RecipeId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSourceRecipes", x => new { x.RecipeId, x.SourceId });
                    table.UniqueConstraint("AK_RecipeSourceRecipes_RecipeId", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_RecipeSourceRecipes_Recipes_RecipeId1",
                        column: x => x.RecipeId1,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeSourceRecipes_RecipeBaseSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "RecipeBaseSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    RecipeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SourceId",
                table: "Recipes",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImages_RecipeId",
                table: "RecipeImages",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngrediants_MemberId",
                table: "RecipeIngrediants",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRecipeTag_RecipeId1",
                table: "RecipeRecipeTag",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeRecipeTag_RecipeTagId",
                table: "RecipeRecipeTag",
                column: "RecipeTagId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_RecipeId1",
                table: "RecipeSourceRecipes",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSourceRecipes_SourceId",
                table: "RecipeSourceRecipes",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_RecipeId",
                table: "RecipeSteps",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeImages");

            migrationBuilder.DropTable(
                name: "RecipeIngrediants");

            migrationBuilder.DropTable(
                name: "RecipeRecipeTag");

            migrationBuilder.DropTable(
                name: "RecipeSourceRecipes");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "Ingrediants");

            migrationBuilder.DropTable(
                name: "RecipeTags");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "RecipeBaseSources");
        }
    }
}