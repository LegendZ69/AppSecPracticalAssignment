using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppSecPracticalAssignment.Model;
using AppSecPracticalAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using System.Net;
using System.Text.Json.Nodes;

namespace AppSecPracticalAssignment.Pages
{
    //Initialize the build-in ASP.NET Identity
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
/*        private readonly SignInManager<ApplicationUser> signInManagerAPP;
*/        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public Register RModel { get; set; }

        /*Practical 13 Customising ASPNetUser*/
        /*public RegisterModel(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> roleManager, services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>())
        {
            this.userManager = userManager;
            this.signInManagerAPP = signInManagerAPP;
            this.roleManager = roleManager;
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(IFormCollection fc)
        {
            string res = fc["FirstName"];
            return View();
        }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public static bool ReCaptchaPassed(string gRecaptchaResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Ld0FxwpAAAAAEjsQEaEXrNxvEQSgnhCEVs9u_3L&response={gRecaptchaResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JsonObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
            {
                return false;
            }

            return true;
        }

        public void OnGet()
        {
        }

        //Save data into the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");

                var user = new IdentityUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    PhoneNumber = RModel.MobileNo
                };

                /*var user = new ApplicationUser()
                {
                    *//*                    UserName = RModel.FirstName + " " + RModel.LastName,*//*
                    UserName = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    CreditCardNo = protector.Protect(RModel.CreditCardNo),
                    MobileNo = RModel.MobileNo,
                    ShippingAddress = RModel.ShippingAddress,
                    BillingAddress = RModel.BillingAddress,
                    Email = RModel.Email,
                    Photo = RModel.Photo
                };*/

                //Create the User role if NOT exist
                IdentityRole role = await roleManager.FindByIdAsync("User");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("User"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role user failed");
                    }
                }

                var result = await userManager.CreateAsync(user, RModel.Password);

                if (result.Succeeded)
                {
                    //Add users to User Role
                    result = await userManager.AddToRoleAsync(user, "User");

                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                if (!ReCaptchaPassed(Request.Form["g-recaptcha"]))
                {
                    ModelState.AddModelError(string.Empty, "CAPTCHA Failed.");
                }
            }
            return Page();
        }

    }
}
