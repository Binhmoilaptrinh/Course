using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Admin.Course
{
    public class CourseManageModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
    }
}
