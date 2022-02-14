using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using SeaBattle.AuthorizationService.IdentityServer.Helpers;
using SeaBattle.AuthorizationService.IdentityServer.Options;
using SeaBattle.AuthorizationService.IdentityServer.UI;
using SeaBattle.AuthorizationService.IdentityServer.ViewModels.Account;
using SeaBattle.AuthorizationService.Models;
using Serilog;

namespace SeaBattle.AuthorizationService.IdentityServer.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IIdentityServerInteractionService _interaction;

        private readonly IClientStore _clientStore;

        private readonly IAuthenticationSchemeProvider _schemeProvider;

        private readonly IEventService _events;

        private readonly IAccountControllerHelper _helper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events, IAccountControllerHelper helper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _helper = helper;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await _helper.BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId:context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await _helper.BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await _helper.BuildLogoutViewModelAsync(this.User, logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await _helper.BuildLoggedOutViewModelAsync(this.User, model.LogoutId, this.HttpContext);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Registration(string returnUrl)
        {
            return View(new RegistrationInputModel() {ReturnUrl = returnUrl});
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "register")
            {
                if (context != null)
                {
                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(model.Username).Result;

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        EmailConfirmed = false
                    };

                    var resultCreate = _userManager.CreateAsync(user, model.Password).Result;

                    if (!resultCreate.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty,
                            string.Join("\n", resultCreate.Errors.Select(e => e.Description)));
                        return View(model);
                    }

                    resultCreate = _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.NickName, model.Username)
                    }).Result;

                    if (!resultCreate.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty,
                            string.Join("\n", resultCreate.Errors.Select(e => e.Description)));
                        return View(model);
                    }


                    var result = _userManager.AddToRoleAsync(user, "user_role").Result;

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty,
                            string.Join("\n", resultCreate.Errors.Select(e => e.Description)));
                        return View(model);
                    }

                    return RedirectToAction("Login", "Account", new {returnUrl = model.ReturnUrl});
                }
                else
                {
                    ModelState.AddModelError("Username", AccountOptions.UserAlreadyExists);
                    return View(model);
                }
            }

            // something went wrong, show form with error
            return View(model);
        }
    }
}
