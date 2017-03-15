using System.ComponentModel.DataAnnotations;

namespace HIS.WebApi.Auth.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
