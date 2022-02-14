using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SeaBattle.AuthorizationService.IdentityServer.ViewModels.Account;

namespace SeaBattle.AuthorizationService.IdentityServer.Helpers
{
    public interface IAccountControllerHelper
    {
        Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);

        Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model);

        Task<LogoutViewModel> BuildLogoutViewModelAsync(ClaimsPrincipal user, string logoutId);

        Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(ClaimsPrincipal user, string logoutId,
            HttpContext context);
    }
}
