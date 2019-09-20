using System.ComponentModel.DataAnnotations;

namespace Confetti.Identity.ViewModels
{
    public class LoginInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
    }
}