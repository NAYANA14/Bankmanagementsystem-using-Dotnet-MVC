#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankManagement.Models;
using BankManagement.ViewModels;

namespace bankmanagementsystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankManagementDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CustomerController(BankManagementDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var bankManagementDbContext = _context.Customers.Include(c => c.User);
            return View(await bankManagementDbContext.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customerExists = await _context.Customers
              .FirstOrDefaultAsync(m => m.UserId == HttpContext.Session.GetInt32("UserId"));
                if (customerExists == null)
                {
                    string uniqueFileName = UploadedFile(model);
                    string uniquesignature = Uploadsiganture(model);
                    Customer customer = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        AdharNumber = model.AdharNumber,
                        PanNumber = model.PanNumber,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        DateOfBirth = model.DateOfBirth,
                        Image = uniqueFileName,
                        Signature = uniquesignature,
                    };
                    customer.UserId = HttpContext.Session.GetInt32("UserId");
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    Account account = new Account
                    {
                        AccountNumber = AccNumber(10000, 50000),
                        Type = model.AccountType,
                        Balance = 3000,
                    };
                    account.CustomerId = customer.Id;
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Account account = new Account
                    {
                        AccountNumber = AccNumber(10000, 50000),
                        Type = model.AccountType,
                        Balance = 3000,
                    };
                    account.CustomerId = customerExists.Id;
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Home));
            }

            return View();
        }
        public int AccNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string UploadedFile(CustomerViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private string Uploadsiganture(CustomerViewModel model)
        {
            string uniquesignature = null;

            if (model.Signature != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniquesignature = Guid.NewGuid().ToString() + "_" + model.Signature.FileName;
                string filePath = Path.Combine(uploadsFolder, uniquesignature);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Signature.CopyTo(fileStream);
                }
            }
            return uniquesignature;
        }




        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,FullName,Address,AdharNumber,PanNumber,PhoneNumber,Email,DateOfBirth,Image,Signature,UserId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
        
        public async Task<IActionResult> Profile()
        {
            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == HttpContext.Session.GetInt32("UserId"));
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
    }
}
