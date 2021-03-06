﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.Services.Interfaces.Services
{
    public interface ISourceService:IDisposable
    {
        /// <summary>
        /// Returns all recipe source entries
        /// </summary>
        /// <returns></returns>
        IQueryable<SourceListEntryViewModel> GetSources();
        /// <summary>
        /// Returns all cookbook data 
        /// </summary>
        /// <returns></returns>
        IQueryable<CookbookSourceViewModel> GetCookbooks();
        /// <summary>
        /// Removes a recipe source
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveSourceAsync(int id);
        /// <summary>
        /// Creates a new Cookbook
        /// </summary>
        /// <param name="creationModel">data of new source</param>
        /// <returns></returns>
        Task<CookbookSourceViewModel> AddCookbookAsync(CookbookSourceCreationViewModel creationModel);
        /// <summary>
        /// Updates a cookbook
        /// </summary>
        /// <param name="sourceId">id of the db entry</param>
        /// <param name="source">new data</param>
        /// <returns></returns>
        Task UpdateCookbookAsync(int sourceId, CookbookSourceViewModel source);
        /// <summary>
        /// Creates a web source
        /// </summary>
        /// <param name="recipeId">owning recipe id</param>
        /// <param name="source">new Data</param>
        /// <returns></returns>
        Task<WebSourceViewModel> AddWebSourceAsync(int recipeId, WebSourceCreationViewModel source);
        /// <summary>
        /// Updates a web source
        /// </summary>
        /// <param name="recipeId">owning recipe id</param>
        /// <param name="sourceId">web source id</param>
        /// <param name="source">new Data</param>
        /// <returns></returns>
        Task UpdateWebSourceAsync(int recipeId, int sourceId, WebSourceViewModel source);
        /// <summary>
        /// Adds or updates a recipe in a cookbook source
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="sourceId">id of the cookbook</param>
        /// <param name="page">Page within the cookbook</param>
        /// <returns></returns>
        Task<CookbookSourceViewModel> UpdateRecipeOnCookbookAsync(int recipeId, int sourceId, int page);
    }
}
