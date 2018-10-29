using System;
using System.Linq;
using System.Threading.Tasks;
using DutchTreatTwo.Data;
using DutchTreatTwo.Services;
using DutchTreatTwo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreatTwo.Controllers
{
   public class AppController : Controller
   {
      private readonly IMailService _mailService;
      private readonly IDutchRepository _dutchRepository;
      

      public AppController(IMailService mailService, IDutchRepository dutchRepository)
      {
         _mailService = mailService;
         _dutchRepository = dutchRepository;
      }
      public IActionResult Index()
      {
         return View();
      }

      [HttpGet]
      public IActionResult Contact()
      {
         ViewBag.Title = "Contact Us";
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult Contact(ContactViewModel model)
      {
         if (ModelState.IsValid)
         {
            //send the mail
            _mailService.SendMail("vibsbali@gmail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message {model.Message}");
            return RedirectToAction("Index");
         }

         return View(model);
      }

      public IActionResult About()
      {
         ViewBag.Title = "About Us";
         return View();
      }

      [Authorize]
      public async Task<IActionResult> Shop()
      {
         var results = await _dutchRepository.GetAllProducts();
         return View(results);
      }
   }
}
