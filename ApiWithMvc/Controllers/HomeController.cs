using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiWithMvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            List<Contact> contacts = Contact.GetContacts();
            ViewBag.Contacts = new SelectList(contacts, "Phone_Number", "Friendly_Name");
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Message newMessage)
        {
            newMessage.From = "+19712137542";
            newMessage.Send();
            return RedirectToAction("Index");
        }
    }
}
