using CodingAddaSurfaceController.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace CodingAddaSurfaceController.Controller
{
	public class ContactUsController : SurfaceController
	{
		public ContactUsController(
			IUmbracoContextAccessor umbracoContextAccessor,
			IUmbracoDatabaseFactory databaseFactory,
			ServiceContext services,
			AppCaches appCaches,
			IProfilingLogger profilingLogger,
			IPublishedUrlProvider publishedUrlProvider)
			: base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult HandleSubmit(ContactUs contactModel)
		{
			if (!ModelState.IsValid)
			{
				// Return to the current page with validation errors
				return CurrentUmbracoPage();
			}

			// Send email
			try
			{
				SendEmail(contactModel);
				TempData["Success"] = "Page submitted successfully with " + contactModel.Name;
			}
			catch (Exception ex)
			{
				TempData["Error"] = "There was an error sending your message: " + ex.Message;
			}

			return RedirectToCurrentUmbracoPage();
		}

		private void SendEmail(ContactUs model)
		{
			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential("bheemala.vijay@gmail.com", "pufh pbtw ogzd vmnx"), // Use App Password if 2FA is enabled
				EnableSsl = true,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(model.Email),
				Subject = "New Contact Form Submission",
				Body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}",
				IsBodyHtml = false,
			};

			mailMessage.To.Add("vijay.bheemala@websynergies.com"); // Change to your recipient email

			smtpClient.Send(mailMessage);
		}
	}
}
