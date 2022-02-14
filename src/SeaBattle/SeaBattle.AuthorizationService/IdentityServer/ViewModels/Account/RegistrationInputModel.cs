using System.ComponentModel.DataAnnotations;

namespace SeaBattle.AuthorizationService.IdentityServer.ViewModels.Account
{
    public class RegistrationInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string ReturnUrl { get; set; }
    }
}
