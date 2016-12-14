using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Services.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    [Migration("20161213152020_ChangedUniqueKeyDefinitions")]
    partial class ChangedUniqueKeyDefinitions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HIS.Recipes.Services.Models.Ingrediant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Ingrediants");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Calories");

                    b.Property<int>("CookedCounter");

                    b.Property<string>("Creator")
                        .IsRequired();

                    b.Property<DateTime>("LastTimeCooked");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("NumberOfServings");

                    b.Property<int>("SourceId");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeBaseSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("RecipeBaseSources");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RecipeBaseSource");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Filename")
                        .IsRequired();

                    b.Property<int>("RecipeId");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeImages");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeIngrediant", b =>
                {
                    b.Property<int>("RecipeId");

                    b.Property<int>("IngrediantId");

                    b.Property<int>("Amount");

                    b.Property<int>("CookingUnit");

                    b.HasKey("RecipeId", "IngrediantId");

                    b.HasIndex("IngrediantId");

                    b.ToTable("RecipeIngrediants");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeRecipeTag", b =>
                {
                    b.Property<int>("RecipeId");

                    b.Property<int>("RecipeTagId");

                    b.HasKey("RecipeId", "RecipeTagId");

                    b.HasIndex("RecipeTagId");

                    b.ToTable("RecipeRecipeTags");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeSourceRecipe", b =>
                {
                    b.Property<int>("RecipeId");

                    b.Property<int>("SourceId");

                    b.Property<int>("Page");

                    b.HasKey("RecipeId", "SourceId");

                    b.HasIndex("RecipeId")
                        .IsUnique();

                    b.HasIndex("SourceId");

                    b.HasIndex("RecipeId", "SourceId", "Page")
                        .IsUnique();

                    b.ToTable("RecipeSourceRecipes");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<int>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId", "Order")
                        .IsUnique();

                    b.ToTable("RecipeSteps");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("RecipeTags");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeCookbookSource", b =>
                {
                    b.HasBaseType("HIS.Recipes.Services.Models.RecipeBaseSource");

                    b.Property<string>("ISBN")
                        .IsRequired();

                    b.Property<string>("PublishingCompany")
                        .IsRequired();

                    b.ToTable("RecipeCookbookSource");

                    b.HasDiscriminator().HasValue("RecipeCookbookSource");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeUrlSource", b =>
                {
                    b.HasBaseType("HIS.Recipes.Services.Models.RecipeBaseSource");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.ToTable("RecipeUrlSource");

                    b.HasDiscriminator().HasValue("RecipeUrlSource");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeImage", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany("Images")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeIngrediant", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Ingrediant", "Ingrediant")
                        .WithMany("Recipes")
                        .HasForeignKey("IngrediantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany("Ingrediants")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeRecipeTag", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany("Tags")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HIS.Recipes.Services.Models.RecipeTag", "RecipeTag")
                        .WithMany("Recipes")
                        .HasForeignKey("RecipeTagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeSourceRecipe", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithOne("Source")
                        .HasForeignKey("HIS.Recipes.Services.Models.RecipeSourceRecipe", "RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HIS.Recipes.Services.Models.RecipeBaseSource", "Source")
                        .WithMany("RecipeSourceRecipes")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeStep", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
