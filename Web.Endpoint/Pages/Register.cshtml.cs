using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Endpoint.Models.ViewModels.Register;

namespace Web.Endpoint.Pages
{
    public class RegisterModel : PageModel
    {
        public RegisterViewModel Command;
        private readonly UserManager<User> _userManager;

        public RegisterModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
          
        }

        public IActionResult OnPost(RegisterViewModel command)
        {
            if (!ModelState.IsValid) return Page();

            var user = new User
            {
                UserName = command.Email,
                Email = command.Email,
                Fullname = command.FullName,
                PhoneNumber = command.PhoneNumber,

            };

            var result = _userManager.CreateAsync(user, command.Password).Result;
            if (result.Succeeded)
            {
                return Page();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    return Page();
                }
                return Page();
            }
        }

    }
}
