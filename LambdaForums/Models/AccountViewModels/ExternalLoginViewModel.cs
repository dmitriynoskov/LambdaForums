using System.ComponentModel.DataAnnotations;

namespace LambdaForums.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
