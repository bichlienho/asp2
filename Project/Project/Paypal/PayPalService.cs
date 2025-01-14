using PayPal.Api;
using System.Collections.Generic;

public class PayPalService
{
    private readonly IConfiguration _config;

    public PayPalService(IConfiguration config)
    {
        _config = config;
    }

    public APIContext GetApiContext()
    {
        // Sử dụng OAuthTokenCredential để lấy AccessToken
        var oauthTokenCredential = new OAuthTokenCredential(
            _config["PayPal:ClientId"],
            _config["PayPal:ClientSecret"]);

        string accessToken = oauthTokenCredential.GetAccessToken(); // Lấy token truy cập

        // Khởi tạo APIContext với access token
        var apiContext = new APIContext(accessToken)
        {
            Config = new Dictionary<string, string>()
            {
                { "mode", _config["PayPal:Mode"] } // sandbox or live
            }
        };

        return apiContext;
    }
    public Payment CreatePayment(string baseUrl, string intent, List<Transaction> transactions, string returnUrl, string cancelUrl)
    {
        var apiContext = GetApiContext();

        var payment = new Payment
        {
            intent = intent,
            payer = new Payer { payment_method = "paypal" },
            transactions = transactions,
            redirect_urls = new RedirectUrls
            {
                cancel_url = $"{baseUrl}{cancelUrl}",
                return_url = $"{baseUrl}{returnUrl}"
            }
        };

        return payment.Create(apiContext); // Thực hiện thanh toán
    }

}
