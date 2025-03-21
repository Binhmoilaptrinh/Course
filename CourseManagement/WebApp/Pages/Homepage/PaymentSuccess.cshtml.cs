using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Pages.Homepage
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public PaymentSuccessModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get orderCode from the URL query parameter
            if (!Request.Query.TryGetValue("orderCode", out var orderCodeValue) ||
                !long.TryParse(orderCodeValue, out long orderCode))
            {
                return BadRequest("Invalid or missing order code.");
            }

            // Call the API to update payment status
            var response = await _httpClient.PostAsync($"https://api.2handshop.id.vn/api/Payments/UpdateSuccess?orderCode={orderCode}", null);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to update payment.");
            }

            return Page();
        }
    }
}
