using System.ComponentModel.DataAnnotations;

namespace SeaBattle.AuthorizationService.IdentityServer.ViewModels.Account
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}
