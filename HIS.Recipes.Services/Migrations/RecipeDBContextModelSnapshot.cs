using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HIS.Recipes.Services;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Enums;

namespace HIS.Recipes.Services.Migrations
{
    [DbContext(typeof(RecipeDBContext))]
    partial class RecipeDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HIS.Recipes.Services.Models.Ingrediant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Ingrediants");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Calories");

                    b.Property<int>("CookedCounter");

                    b.Property<string>("Creator");

                    b.Property<DateTime>("LastTimeCooked");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("NumberOfServings");

                    b.Property<Guid>("SourceId");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeBaseSource", b =>
                {
                    b.Property<Guid>("Id")
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Filename")
                        .IsRequired();

                    b.Property<Guid>("RecipeId");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeImages");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeIngrediant", b =>
                {
                    b.Property<Guid>("RecipeId");

                    b.Property<Guid>("IngrediantId");

                    b.Property<int>("Amount");

                    b.Property<int>("CookingUnit");

                    b.Property<Guid?>("MemberId");

                    b.HasKey("RecipeId", "IngrediantId");

                    b.HasAlternateKey("RecipeId");


                    b.HasAlternateKey("IngrediantId", "RecipeId");

                    b.HasIndex("MemberId");

                    b.ToTable("RecipeIngrediants");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeRecipeTag", b =>
                {
                    b.Property<Guid>("RecipeId");

                    b.Property<Guid>("RecipeTagId");

                    b.Property<Guid?>("RecipeId1");

                    b.HasKey("RecipeId", "RecipeTagId");

                    b.HasAlternateKey("RecipeId");

                    b.HasIndex("RecipeId1");

                    b.HasIndex("RecipeTagId");

                    b.ToTable("RecipeRecipeTag");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeSourceRecipe", b =>
                {
                    b.Property<Guid>("RecipeId");

                    b.Property<Guid>("SourceId");

                    b.Property<int?>("Page");

                    b.Property<Guid?>("RecipeId1");

                    b.HasKey("RecipeId", "SourceId");

                    b.HasAlternateKey("RecipeId");

                    b.HasIndex("RecipeId1");

                    b.HasIndex("SourceId");

                    b.ToTable("RecipeSourceRecipes");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeStep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<Guid>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeSteps");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

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

            modelBuilder.Entity("HIS.Recipes.Services.Models.Recipe", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.RecipeCookbookSource", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("HIS.Recipes.Services.Models.Ingrediant", "Comment")
                        .WithMany()
                        .HasForeignKey("IngrediantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Member")
                        .WithMany("Ingrediants")
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeRecipeTag", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany("Tags")
                        .HasForeignKey("RecipeId1");

                    b.HasOne("HIS.Recipes.Services.Models.RecipeTag", "RecipeTag")
                        .WithMany("Recipes")
                        .HasForeignKey("RecipeTagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HIS.Recipes.Services.Models.RecipeSourceRecipe", b =>
                {
                    b.HasOne("HIS.Recipes.Services.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId1");

                    b.HasOne("HIS.Recipes.Services.Models.RecipeBaseSource", "Source")
                        .WithMany()
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
