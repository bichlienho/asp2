using BCrypt.Net;
using Project.Models;
using Project.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Repository;
using Microsoft.Extensions.Options;
using Project.EmailService;
using Microsoft.AspNetCore.Identity;

namespace Day6.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailService _emailService; // IEmailService for sending emails
		private readonly MailSettings _mailSettings; // MailSettings for configuration
		private readonly IAccountRepository accountRepo;
        private readonly DatabaseContext db;
        SecurityManager securityManager = new SecurityManager();
        public AccountController(IAccountRepository accountRepo, DatabaseContext db,  IEmailService emailService, IOptions<MailSettings> mailSettings)
        {
            this.accountRepo = accountRepo;
			_emailService = emailService; // Injecting IEmailService
			_mailSettings = mailSettings.Value; // Injecting MailSettings
            this.db = db;

        }
        // GET: Login page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Kiểm tra đăng nhập thông qua tài khoản và mật khẩu
            var account = await accountRepo.CheckLogin(email, password);

            // Kiểm tra xem model có hợp lệ không
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            // Nếu không tìm thấy tài khoản hoặc tài khoản không hợp lệ
            if (account == null)
            {
                ViewBag.Error = "\r\nEmail or Password is incorrect. Please re-enter!";
                return View("Login");  // Quay lại trang đăng nhập
            }

            // Nếu mật khẩu sai
            if (!BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                ViewBag.Error = "Incorrect password.";
                return View("Index");  // Quay lại trang đăng nhập
            }

            // Tiến hành đăng nhập người dùng
            await securityManager.SignIn(this.HttpContext, account);

            // Xác định vai trò của tài khoản
            var role = account.Role?.Name;

            // Chuyển hướng người dùng đến trang thích hợp dựa trên vai trò
            if (role == "User")
            {
                TempData["SuccessMessage"] = "Login successful! Welcome User.";
                return RedirectToAction("Index", "Web");  // Trang chủ người dùng
            }
            else if (role == "Admin")
            {
                TempData["SuccessMessage"] = "Login successful! Welcome Admin.";
                return Redirect("/Admin");  // Trang quản trị viên
            }
            else
            {
                ViewBag.Error = "No valid role assigned.";
                return View("Index");  // Trở lại trang đăng nhập nếu không có vai trò hợp lệ
            }
        }


        public IActionResult Welcome()
        {
            return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]  // Prevent CSRF attacks
		public async Task<IActionResult> SignOut()
		{
			// Clear the user claims and sign the user out
			await securityManager.SignOut(this.HttpContext);

			// Optionally, you can set a confirmation message for the user
			TempData["SuccessMessage"] = "You have successfully logged out.";

			// Redirect to the home page or login page
			return RedirectToAction("Login", "Account");
		}

		public IActionResult AccessDenied()
        {
            return View();  
        }
        public IActionResult User()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account account, string confirmPassword)
        {
            // Check if passwords match
            if (account.Password != confirmPassword)
            {
                ViewBag.Error = "Confirm Password does not match password.";
                return View();
            }

            try
            {
                // Attempt to register the account
                var newAccount = await accountRepo.Register(account);

                if (newAccount != null)
                {
                    // Prepare email content
                    var emailBody = $@"
<div style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 600px; margin: auto; background-color: #f9f9f9;"">
    <div style=""text-align: center; margin-bottom: 20px;"">
        <h1 style=""color: #007BFF;"">Welcome to Our Service!</h1>
        <p style=""font-size: 14px; color: #555;"">Thank you for registering with us. Your account has been successfully created.</p>
    </div>
    <div style=""padding: 15px; border: 1px solid #eee; border-radius: 8px; background-color: white;"">
        <p style=""font-size: 16px;"">Here are your account details:</p>
        <p style=""font-size: 16px;""><strong>Username:</strong> {account.Username}</p>
        <p style=""font-size: 16px;""><strong>Password:</strong> {account.Password}</p>
        <p style=""font-size: 14px; margin-top: 20px;"">You can now log in to your account and explore our services. Please remember to keep your account details safe.</p>
    </div>
    <div style=""margin-top: 20px; text-align: center;"">
        <a href=""https://yourwebsite.com/login"" style=""display: inline-block; padding: 10px 20px; background-color: #007BFF; color: white; text-decoration: none; font-size: 16px; border-radius: 8px; transition: background-color 0.3s;"" onmouseover=""this.style.backgroundColor='#0056b3';"" onmouseout=""this.style.backgroundColor='#007BFF';"">Log in Now</a>
    </div>
    <hr style=""border: none; border-top: 1px solid #eee; margin: 20px 0;"">
    <div style=""text-align: center;"">
        <p style=""font-size: 14px; color: #777;"">If you have any questions, feel free to reach out to our support team.</p>
        <a href=""mailto:support@example.com"" style=""color: #007BFF; text-decoration: none;"">Contact Support</a>
    </div>
</div>";

                    // Send email
                    await _emailService.SendEmailAsync(account.Email, "Account Created Successfully", emailBody);

                    // Redirect to login page after successful registration
                    return RedirectToAction("Login");
                }

                ViewBag.Error = "Failed to register account.";
                return View();
            }
            catch (Exception ex)
            {
                // Show the specific error message
                ViewBag.Error = ex.Message;
                return View();  // Return to the registration page
            }
        }
        //forget
        [HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		// POST: ForgotPassword
		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				ViewBag.ErrorMessage = "Vui lòng nhập email.";
				return View();
			}

			try
			{
				// Find the account by email
				var account = await db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
				if (account == null)
				{
					ViewBag.ErrorMessage = "Email không tồn tại trong hệ thống.";
					return View();
				}

				// Generate a new password
				string newPassword = GenerateRandomPassword();

				// Hash the new password before saving it
				account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
				await db.SaveChangesAsync();
                // Create email body
                var emailBody = $@"
<div style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333; padding: 20px; border: 1px solid #ddd; border-radius: 10px; max-width: 600px; margin: auto; background-color: #f9f9f9;"">
    <div style=""text-align: center; margin-bottom: 20px;"">
        <h1 style=""color: #007BFF;"">Password Reset</h1>
        <p style=""font-size: 14px; color: #555;"">Your account password has been reset successfully.</p>
    </div>
    <div style=""padding: 15px; border: 1px solid #eee; border-radius: 8px; background-color: white;"">
        <p style=""font-size: 16px;"">Hello <strong>{account.Fullname}</strong>,</p>
        <p style=""font-size: 16px;"">Your new password is:</p>
        <p style=""font-size: 20px; font-weight: bold; color: #007BFF; text-align: center; padding: 10px 0;"">{newPassword}</p>
        <p style=""font-size: 14px;"">Please <strong>log in</strong> and change your password immediately to ensure the safety of your account.</p>
    </div>
    <hr style=""border: none; border-top: 1px solid #eee; margin: 20px 0;"">
    <div style=""text-align: center;"">
        <p style=""font-size: 14px; color: #777;"">If you did not request this change, please contact our support team immediately.</p>
        <a href=""mailto:support@example.com"" style=""color: #007BFF; text-decoration: none;"">Contact Support</a>
    </div>
</div>";


                // Send email
                await _emailService.SendEmailAsync(account.Email, "\r\nYour new password is:", emailBody);

				ViewBag.SuccessMessage = "New password has been sent to your email!.";
				return View();
			}
			catch (Exception ex)
			{
				// Log the error message
				ViewBag.ErrorMessage = $"ERROR {ex.Message}";
				return View();
			}
		}

		// Helper: Sinh mật khẩu ngẫu nhiên
		private string GenerateRandomPassword()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
		}



	}
}
