using SeaBattle.AuthorizationService.IdentityServer.ViewModels.Consent;

namespace SeaBattle.AuthorizationService.IdentityServer.ViewModels.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}
