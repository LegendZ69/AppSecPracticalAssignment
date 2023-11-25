using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppSecPracticalAssignment.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        private readonly IHttpContextAccessor contxt;
        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            contxt = httpContextAccessor;
        }

        /*public IActionResult Index()
        {
            contxt.HttpContext.Session.SetString("FirstName", RModel.FirstName);
            return View();
        }*/


    }
}
