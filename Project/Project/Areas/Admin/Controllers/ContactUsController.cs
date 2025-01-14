using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project.Data;
using Project.EmailService;
using Project.Models;
using Project.Repository;
using System.Net.Mail;

namespace Project.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[controller]/[action]")]
	public class ContactUsController : Controller

	{
		private readonly IAccountRepository contactRepo;
		private readonly DatabaseContext db;
		private readonly IEmailService _emailService; // IEmailService for sending emails
		private readonly MailSettings _mailSettings; // MailSettings for configuration

		// Constructor that injects IAccountRepository, DatabaseContext, IEmailService, and MailSettings
		public ContactUsController(IAccountRepository contactRepo, DatabaseContext db, IEmailService emailService, IOptions<MailSettings> mailSettings)
		{
			this.contactRepo = contactRepo;
			this.db = db;
			_emailService = emailService; // Injecting IEmailService
			_mailSettings = mailSettings.Value; // Injecting MailSettings
		}
		public ActionResult Index()
		{

			var contacts = db.ContactUs.ToList();
			return View(contacts); // Passing the correct model
		}


		// GET: ContactUs/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: ContactUs/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ContactUs contact)
		{
			if (ModelState.IsValid)
			{
				db.ContactUs.Add(contact);
				db.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(contact);
		}

        //Edit
        // GET: ContactUs/Edit/5
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var contact = db.ContactUs.Find(id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        // POST: Admin/ContactUs/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactUs contact)
        {
            if (id != contact.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(contact);
                    await db.SaveChangesAsync();

                    var emailBody = $@"
<div style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background: #ffffff; border: 1px solid #ddd; border-radius: 10px; padding: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
<h2 style='color: #4CAF50; text-align: center;'>🎉 Your Contact Information Has Been Updated 🎉</h2>
        <p style='font-size: 16px; color: #333;'>Dear <strong>{contact.Name}</strong>,</p>
        <p style='font-size: 16px; color: #333;'>We are thrilled to inform you that your contact details have been updated successfully in our system. Here is the updated information:</p>
        <ul style='font-size: 16px; color: #555; list-style: none; padding: 0;'>
            <li style='margin-bottom: 10px;'><strong>📞 Phone:</strong> {contact.Phone}</li>
            <li style='margin-bottom: 10px;'><strong>📧 Email:</strong> {contact.Email}</li>
            <li style='margin-bottom: 10px;'><strong>💬 Reply:</strong> {contact.Comment}</li>
        </ul>
        <p style='font-size: 16px; color: #333;'>Thank you for staying in touch with us! If you have any questions or need further assistance, feel free to reach out anytime.</p>
        <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
        <p style='text-align: center; font-size: 14px; color: #888;'>&copy; {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
    </div>
</div>";

                    await _emailService.SendEmailAsync(contact.Email, "Contact Information Updated", emailBody);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later.");
                }
            }
            return View(contact);
        }

        [HttpGet("admin/contactus/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await db.ContactUs.FindAsync(id);
            if (contact == null)
            {
                return RedirectToAction("Index");
            }

            return View(contact); // Confirmation page
        }
        [HttpPost("admin/contactus/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var contact = await db.ContactUs.FindAsync(id);
            if (contact != null)
            {
                db.ContactUs.Remove(contact);
                await db.SaveChangesAsync();
                TempData["Note"] = "Contact Us deleted successfully!";
            }
            return RedirectToAction("Index");
        }

    }
}