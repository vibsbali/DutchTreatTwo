using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DutchTreatTwo.Data.Entities;
using DutchTreatTwo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DutchTreatTwo.Controllers
{
   public class AccountController : Controller
   {
      private readonly ILogger<AccountController> _logger;
      private readonly SignInManager<StoreUser> _signInManager;
      private readonly UserManager<StoreUser> _userManager;
      private readonly IConfiguration _configuration;

      public AccountController(ILogger<AccountController> logger,
         SignInManager<StoreUser> signInManager,
         UserManager<StoreUser> userManager,
         IConfiguration configuration)
      {
         _logger = logger;
         _signInManager = signInManager;
         _userManager = userManager;
         _configuration = configuration;
      }

      public IActionResult Login()
      {
         if (this.User.Identity.IsAuthenticated)
         {
            return RedirectToAction("Index", "App");
         }

         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginViewModel viewModel)
      {
         if (ModelState.IsValid)
         {
            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, false);

            if (result.Succeeded)
            {
               //this checks if return url was present
               if (Request.Query.Keys.Contains("ReturnUrl"))
               {
                  return Redirect(Request.Query["ReturnUrl"].First());
               }

               return RedirectToAction("Index", "App");
            }
         }

         ModelState.AddModelError("", "Failed to Login");

         return View();
      }

      [HttpPost]
      public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
      {
         if (ModelState.IsValid)
         {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
               var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

               if (result.Succeeded)
               {
                  //create claims
                  var claims = new[]
                  {
                     new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                  };

                  //create a symmetric key
                  var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                  var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                  //create JwtSecurityToken
                  var token = new JwtSecurityToken(
                     _configuration["Tokens:Issuer"],
                     _configuration["Tokens:Audience"],
                     claims,
                     expires:DateTime.UtcNow.AddMinutes(30),
                     signingCredentials: creds
                     );

                  //create a token that is to be returned
                  var results = new
                  {
                     token = new JwtSecurityTokenHandler().WriteToken(token),
                     expiration = token.ValidTo
                  };

                  return Created("", results);
               }
            }
         }

         return BadRequest();
      }

      public async Task<IActionResult> LogoutAsync()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction("Index", "App");
      }
   }
}
