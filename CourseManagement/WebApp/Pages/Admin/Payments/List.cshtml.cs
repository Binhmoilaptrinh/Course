using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Pages.Homepage;
using X.PagedList;
using X.PagedList.Extensions;
    
namespace WebApp.Pages.Admin.Payments
{
    public class ListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public ListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public List<PaymentListResponse> Payments { get; set; }


        public IPagedList<PaymentListResponse> PagedPayments { get; set; }
        public string CurrentFilter { get; set; } = "";
        public string CurrentSort { get; set; }
        public int? PageSize { get; set; }
        public int? PageNo { get; set; }
        public int? TotalPage { get; set; }
        public async Task OnGetAsync(DateTime? fromDate, DateTime? toDate, string orderNumber, int? status, int? pageNo, int? pageSize)
        {
            pageNo ??= 1;
            pageSize ??= 5;

            var apiUrl = $"https://api.2handshop.id.vn/api/Payments/SearchPayments?fromDate={fromDate}&status={status}&toDate={toDate}&orderNumber={orderNumber}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Payments = JsonSerializer.Deserialize<List<PaymentListResponse>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var paymentsQuery = Payments;

            PagedPayments = paymentsQuery.ToPagedList((int)pageNo, (int)pageSize);

            int totalItems = paymentsQuery.Count();
            int morePage = totalItems % pageSize != 0 ? 1 : 0;
            TotalPage = (totalItems / pageSize) + morePage;

            PageNo = pageNo;
            PageSize = pageSize;
        }

    }
}
