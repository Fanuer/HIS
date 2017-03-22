using System.ComponentModel.DataAnnotations;
using HIS.Bot.WebApi.Data.ViewModels.Interfaces;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    /// <summary>
    /// Returns an entity with its name, id and url
    /// </summary>
    public class NamedViewModel:IViewModelEntity<int>, INamedEntity<int>
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
