using SeaBattle.AuthorizationService.IdentityServer.ViewModels.Consent;

namespace SeaBattle.AuthorizationService.IdentityServer.ViewModels.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }

        public bool ConfirmUserCode { get; set; }
    }
}
