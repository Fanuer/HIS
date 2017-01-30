using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HIS.Bot.WebApi.Data.ViewModels.Enum;

namespace HIS.Bot.WebApi.Extensions
{
    public static  class EnumExtensions
    {
        public static string GetName(this Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }

        public static string GetUnit(this CookingUnit unit)
        {
            var result = "";

            var displayAttributes = unit.GetType().GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>();
            var attributes = displayAttributes as DisplayAttribute[] ?? displayAttributes.ToArray();
            if (attributes.Any())
            {
                result = attributes.First().GetName();
            }
            return result;
        }
    }
}