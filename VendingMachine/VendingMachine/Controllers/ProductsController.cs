using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Data;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    public class ProductsController : Controller
    {
        #region Private Variables

        private readonly VendingMachineContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        #endregion

        #region Constructor

        public ProductsController(VendingMachineContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods

        [Authorize(Roles = "Admin")]

        // GET: Products
        public async Task<IActionResult> Manage()
        {
            return View(await _context.Product.ToListAsync());
        }

        [Authorize(Roles = "User")]

        // GET: Products
        public async Task<IActionResult> Purchase()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,QuantityAvailable")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,QuantityAvailable")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }

        [Authorize(Roles = "User")]
        // POST: Products/Purchase/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Purchase(int id, int quantity)
        //public async Task<IActionResult> Purchase([FromBody]List<Product> productList)
        //public async Task<IActionResult> Purchase([FromBody] JsonElement json)
        public async Task<IActionResult> Purchase([FromBody] List<Product> inputProductList)
        {
            if (inputProductList != null)
            {
                var existingProductList = await _context.Product.ToListAsync();

                foreach (var inputProduct in inputProductList)
                {
                    var existingProduct = existingProductList.FirstOrDefault(x => x.ID == inputProduct.ID);
                    if (existingProduct != null)
                    {
                        if (inputProduct.QuantityAvailable > existingProduct.QuantityAvailable)
                        {
                            return BadRequest("Product not available or insufficient quantity.");

                        }

                        existingProduct.QuantityAvailable -= inputProduct.QuantityAvailable;
                        var userId = _userManager.GetUserId(User);
                        var transaction = new Transaction
                        {
                            ProductID = inputProduct.ID,
                            UserID = userId,
                            TransactionDate = DateTime.Now,
                            Quantity = inputProduct.PurchaseQuantity
                        };

                        _context.Update(existingProduct);
                        _context.Add(transaction);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(Manage));

            //ModelState.AddModelError(string.Empty, "Product not available or insufficient quantity.");
        }

        #endregion
    }
}
