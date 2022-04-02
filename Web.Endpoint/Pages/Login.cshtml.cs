using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Endpoint.Models.ViewModels.Register;

namespace Web.Endpoint.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public LoginViewModel Command;

        public LoginModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

       
        public IActionResult OnGet()
        {
            return Page();
            // Command.ReturnUrl=returnUrl;
        }

        public IActionResult OnPost(LoginViewModel command)
        {
            if (!ModelState.IsValid) return Page();

            var user = _userManager.FindByNameAsync(command.Email).Result;
            if (user != null)
            {
                ModelState.AddModelError("","کاربری یافت نشد");
                return Page();
            }

            _signInManager.SignOutAsync();
            var result = _signInManager.PasswordSignInAsync(user, command.Password, command.IsPersistent, true).Result;
            if (result.Succeeded)
            {
                return Redirect(command.ReturnUrl);
            }

            return Page();
        }


        public IActionResult OnPostLogout()
        {
            _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
