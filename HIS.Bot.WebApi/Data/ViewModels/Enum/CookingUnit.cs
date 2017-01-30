using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels.Enum
{
    public enum CookingUnit
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_None))]
        None,
        /// <summary>
        /// t or tsp. -> Teeloeffel
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Teaspoon))]
        Teaspoon,
        /// <summary>
        /// T, tbl., tbs., or tbsp. -> Essloeffel
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Tablespoon))]
        Tablespoon,
        /// <summary>
        /// ml, cc and mL 
        ///  </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Milliliter))]
        Milliliter,
        /// <summary>
        /// l, litre, L 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Liter))]
        Liter,
        /// <summary>
        /// mg 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Milligramm))]
        Milligramm,
        /// <summary>
        /// g
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Gramm))]
        Gramm,
        /// <summary>
        /// kg 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Kilogramm))]
        Kilogramm,
        /// <summary>
        /// Päckchen 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Package))]
        Package,
        /// <summary>
        /// Bündel 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Bunch))]
        Bunch,
        /// <summary>
        /// Messerspitze 
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EnumValue_CookingUnit_Msp))]
        Msp,
    }
}
