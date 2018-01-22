using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiWithMvc.Models;

namespace ApiWithMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Contact> model = Contact.GetContacts();
            return View(model);
        }

        public IActionResult AddContactForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddContactForm(Contact newContact)
        {
            string validationCode = newContact.GetValidation();
            return Content(validationCode);
        }

        public IActionResult GetMessages()
        {
            var allMessages = Message.GetMessages();
            return View(allMessages);
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Message newMessage)
        {
            newMessage.Send();
            return RedirectToAction("Index");
        }
    }
}
