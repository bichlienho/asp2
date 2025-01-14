using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using Project.Data;
using Project.EmailService;
using Project.Models;
using Microsoft.Extensions.Options;
using System.Net;
[Route("Payment")]
public class PaymentController : Controller
{
    private readonly PayPalService _paypalService;
    private readonly DatabaseContext db;
    private readonly IEmailService _emailService;

    public PaymentController(PayPalService paypalService, DatabaseContext db, IEmailService emailService)
    {
        _paypalService = paypalService;
        this.db = db;
        _emailService = emailService;
    }

    // Tạo thanh toán
    [HttpGet("Checkout")]
	public async Task<IActionResult> Checkout(decimal total)
	{
        var transactions = new List<Transaction>
    {
        new Transaction
        {
            amount = new Amount
            {
                currency = "USD",
                total = total.ToString("F2") 
            },
            description = "Payment Cart"
        }
    };


        var baseUrl = $"{Request.Scheme}://{Request.Host}";
		var payment = _paypalService.CreatePayment(baseUrl, "sale", transactions,
			"/Payment/Success", "/Payment/Cancel");

		var approvalUrl = payment.links.FirstOrDefault(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))?.href;

		return Redirect(approvalUrl);
	}

    // Khi thanh toán thành công
    // Khi thanh toán thành công
    [HttpGet("Success")]
    public async Task<IActionResult> Success(string paymentId, string token, string PayerID, int SkyMiles, string discountCode)
    {
        var apiContext = new APIContext(new OAuthTokenCredential(
            "AVC3gEpeFNZrXaooKS1eVEMoW5bxBjpMs6rMHXCnuP99u80pQFYWMhOH-vmFeNXB_CoGYsFDOaEDcD4z",
            "EJgkiT9jbWfNi9_K-0m_-6DSgeWm7lasJjUUHibfjfl60lcA6fIcxfVmg2j696RhoqnFb80p1UJJyWc2").GetAccessToken());

        var payment = new Payment() { id = paymentId };
        var executedPayment = payment.Execute(apiContext, new PaymentExecution() { payer_id = PayerID });

        if (executedPayment.state.ToLower() != "approved")
        {
            ViewBag.Message = "Payment failed!";
            return View("Failure");
        }

        // Get the account and email
        string username = User.Identity.Name;
        var account = await db.Accounts.FirstOrDefaultAsync(a => a.Username == username);

        if (account != null)
        {
            // Handle Discount Code
            if (!string.IsNullOrEmpty(discountCode))
            {
                var discount = await db.Discounts.SingleOrDefaultAsync(d => d.DiscountCode == discountCode);
                if (discount != null && discount.QuantityDiscount > 0)
                {
                    discount.QuantityDiscount--; // Decrease the quantity
                    db.Discounts.Update(discount);
                    await db.SaveChangesAsync();
                }
            }
            account.SkyMiles += 1; // Increment Skymile by 1
            await db.SaveChangesAsync();

            string toEmail = account.Email;
            if (toEmail != null)
            {
                string subject = "Payment Confirmation";
                string body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 20px auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                <h2 style='text-align: center; color: #4CAF50;'>Payment Confirmation</h2>
                <p>Dear <strong>{account.Fullname}</strong>,</p>
                <p style='font-size: 1.1em;'>We are delighted to inform you that your payment of 
                <strong style='color: #4CAF50;'>${executedPayment.transactions[0].amount.total}</strong> has been successfully processed.</p>
                <p style='font-size: 1.1em;'>Here are the details of your transaction:</p>
                <ul style='font-size: 1em; color: #555;'>
                    <li>Transaction ID: <strong>{executedPayment.id}</strong></li>
                    <li>Amount: <strong>${executedPayment.transactions[0].amount.total}</strong></li>
                    <li>Date: <strong>{DateTime.Now:MMMM dd, yyyy}</strong></li>
                </ul>
                <p style='font-size: 1.1em;'>Thank you for choosing us!</p>
            </div>";
                await _emailService.SendEmailAsync(toEmail, subject, body); // Injected IEmailService
            }
        }

        var carts = await db.Carts.Include(c => c.Flight).Where(c => c.AccId == account.Id).ToListAsync();

        // Add to booking
        foreach (Cart cart in carts)
        {
            Booking booking = new Booking
            {
                FlightId = cart.FlightId,
                Fullname = cart.Fullname,
                Email = cart.Email,
                Dob = cart.Dob,
                IdentityNumber = cart.IdentityNumber,
                Gender = cart.Gender,
                Country = cart.Country,
                AccId = cart.AccId,
                TicketClassId = cart.TicketClassId
            };
            db.Bookings.Add(booking);
        }

        // Remove from cart
        foreach (Cart cart in carts)
        {
            db.Carts.Remove(cart);
        }

        await db.SaveChangesAsync();
        ViewBag.Message = "Payment Successful!";
        return View("Success");
    }

    // Khi hủy thanh toán
    [HttpGet("Cancel")]
	public IActionResult Cancel()
	{
		ViewBag.Message = "Payment has been cancelled!";
		return View("Cancel");
	}
    public async Task<IActionResult> Index()
    {
        var acc = await db.Bookings.ToListAsync();

        return View(acc);
    }
}
