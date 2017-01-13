using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder.Luis.Models;

namespace HIS.Bot.WebApi.Extensions
{
    public static class EntityRecommendationExtensions
    {
        /// <summary>Try to find an entity within the result.</summary>
        /// <param name="luisResult">The LUIS result.</param>
        /// <param name="type">The entity type.</param>
        /// <param name="entities">found entities.</param>
        /// <returns>True if the entity was found, false otherwise.</returns>
        public static bool TryFindEntity(this LuisResult luisResult, string type, out IList<EntityRecommendation> entities)
        {
            type = type ?? "";
            entities = luisResult.Entities.Where(x => x.Type.Equals(type)).ToList();
            return entities.Any();
        }
    }
}
