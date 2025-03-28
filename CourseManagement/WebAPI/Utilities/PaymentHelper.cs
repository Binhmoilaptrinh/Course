using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

namespace WebAPI.Utilities
{
    public class PaymentHelper
    {
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PaymentHelper(IConfiguration configuration)
        {
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:ApiKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
        }

        public  async Task<string> GetLinkAsync(long orderCode, int price, List<ItemData> items)
        {
            PayOS payOS = new(_clientId, _apiKey, _checksumKey);
            string cancelUrl = $"https://course.2handshop.id.vn/Homepage/PaymentFail";
            string returnUrl = "https://course.2handshop.id.vn/Homepage/PaymentSuccess";
            PaymentData paymentData = new PaymentData(orderCode, price, "Thanh toan don hang",
                items, cancelUrl, returnUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

            return createPayment.checkoutUrl;
        }

        public  async Task<bool> VerifyPaymentAsync(long orderCode)
        {
            PayOS payOS = new(_clientId, _apiKey, _checksumKey);
            var paymentStatus = await payOS.getPaymentLinkInformation(orderCode);

            return paymentStatus.status == "PAID";
        }
    }
}