using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Data;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    public class AccountsController : Controller
    {
        #region Private Variables

        private readonly VendingMachineContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        #endregion

        #region Constructor
        public AccountsController(VendingMachineContext context
            , SignInManager<IdentityUser> signInManager
            )
        {
            _context = context;
            _signInManager = signInManager;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //return RedirectToAction("Manage", "Products");
                    if (User.IsInRole("Admin"))
                    {
                        // Logic for Admins
                        return RedirectToAction("Manage", "Products");
                    }
                    else if (User.IsInRole("User"))
                    {
                        // Logic for Users
                        return RedirectToAction("Purchase", "Products");
                    }

                    // Unauthorized access
                    return Forbid();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }

        #endregion
    }
}
