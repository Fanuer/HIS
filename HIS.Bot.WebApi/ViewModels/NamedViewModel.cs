using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.ViewModels
{
    /// <summary>
    /// Returns an entity with its name, id and url
    /// </summary>
    public class NamedViewModel:IViewModelEntity<int>
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
