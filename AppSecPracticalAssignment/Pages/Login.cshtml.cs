using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppSecPracticalAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AppSecPracticalAssignment.Pages
{
	public class LoginModel : PageModel
	{
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<IdentityUser> signInManager;
		public LoginModel(SignInManager<IdentityUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, false, lockoutOnFailure: true);
				if (identityResult.Succeeded)
				{
					//Create the security context
					var claims = new List<Claim> {
						new Claim(ClaimTypes.Email, LModel.Email),

						new Claim("Department", "HR")
};
					var i = new ClaimsIdentity(claims, "MyCookieAuth");
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
					await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

					return RedirectToPage("Index");
				
                else if (IdentityResult.IsLockedOut)
                {
                    // Optionally handle locked out user (e.g., display a message)
                    ModelState.AddModelError("", "Account locked out due to multiple failed attempts.");
                    return Page();
                }
            }
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			
			return Page();
		}
	}
}
