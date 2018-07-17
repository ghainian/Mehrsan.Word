using Microsoft.AspNetCore.Identity;

namespace Mehrsan.Core.Web.Identity.Controllers
{
    public class LoginViewModel
    {
        public IdentityUser Email { get;  set; }
        public bool RememberMe { get;  set; }
        public string Password { get;  set; }
    }
}