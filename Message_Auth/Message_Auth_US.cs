using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScheduleUsers.Models;
using ScheduleUsers.ViewModels;

namespace ScheduleUsers.Controllers
{
    [Authorize(Roles = "Admin,User,ViewMode")]
    public class MessageController : Controller
    {
        //Generate User's first and last name in navigation bar
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    string fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                    ViewData.Add("FullName", fullName);
                    ViewData.Add("ClockedInStatus", user.GetStatus());
                }
            }
            base.OnActionExecuted(filterContext);
        }
        private ApplicationDbContext db = new ApplicationDbContext();

        // Builds the Unread and Read Messages for the current user.
        // GET: Message
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var unreadMessages = db.Messages.Where(m => m.UnreadMessage == true && m.Recipient.Id == userId).OrderByDescending(x => x.DateSent);
            var readMessages = db.Messages.Where(m => m.UnreadMessage == false && m.Recipient.Id == userId).OrderByDescending(x => x.DateSent);
            MessageViewModel messageViewModel = new MessageViewModel(unreadMessages, readMessages); //calls constructor in MessageViewModel
            return View(messageViewModel);
        }

		// Separates the read and unread messages into two sections.
        // GET: Message/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message.UnreadMessage == true)
            {
                message.UnreadMessage = false;

                if (message.DateRead != null)
                {
                    return View(message);
                }
                else
                {
                    message.DateRead = DateTime.Now;
                }
                db.SaveChanges();
            }
		}
	}
}