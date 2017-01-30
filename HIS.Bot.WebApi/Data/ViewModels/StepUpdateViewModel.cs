using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class StepUpdateViewModel : StepCreateViewModel
    {
        [Range(0, int.MaxValue)]
        [Required]
        public int Id { get; set; }
    }
}
