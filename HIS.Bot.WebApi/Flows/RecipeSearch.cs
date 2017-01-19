﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace HIS.Bot.WebApi.Flows
{
    public class RecipeSearch
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS

        public static IForm<RecipeSearch> BuildForm()
        {
            return new FormBuilder<RecipeSearch>()
                    .Message("Welcome to the simple sandwich order bot!")
                    .Build();
        }

        #endregion

        #region PROPERTIES

        public string RecipeName { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Ingrediants { get; set; }

        #endregion

    }
}