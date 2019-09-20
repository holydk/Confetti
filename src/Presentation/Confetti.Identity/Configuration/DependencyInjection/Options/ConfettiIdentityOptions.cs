using System;

namespace Confetti.Identity.Configuration
{
    public class ConfettiIdentityOptions
    {
        public bool AllowLocalLogin = true;
        public bool AllowRememberLogin = false;
        public TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public bool ShowLogoutPrompt = true;
        public bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public bool IncludeWindowsGroups = false;

        public string DefaultReturnUrl { get; set; }

        public string LoginSignInUrlParameter { get; set; }

        public ConfettiIdentityOptions()
        {
            DefaultReturnUrl = "PRODUCTION_URL";
            LoginSignInUrlParameter = "signin";
        }
    }
}