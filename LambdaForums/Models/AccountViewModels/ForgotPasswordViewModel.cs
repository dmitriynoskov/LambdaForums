using System.ComponentModel.DataAnnotations;

namespace LambdaForums.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
