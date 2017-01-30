using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class StepCreateViewModel
    {
        [Range(0, int.MaxValue)]
        public int Order { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
