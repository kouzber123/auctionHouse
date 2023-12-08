using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public Index(ILogger<Index> logger, UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        [BindProperty]
        public bool RegisterSuccess { get; set; }
        public IActionResult OnGet(string returnUrl)
        {
            Input = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            //back to home page if they click cancel
            if(Input.Button != "register") return Redirect("~/");

            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    EmailConfirmed = true,

                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if(result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new Claim[] {
                        new Claim(JwtClaimTypes.Name, Input.FullName)
                    });
                    RegisterSuccess = true;
                }
            }
            return Page();
        }
    }
}
