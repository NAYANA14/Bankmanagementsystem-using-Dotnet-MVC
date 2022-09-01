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
    public class RegisteredPayeeController : Controller
    {
        private readonly BankManagementDbContext _context;

        public RegisteredPayeeController(BankManagementDbContext context)
        {
            _context = context;
        }

        // GET: RegisteredPayee
        public async Task<IActionResult> Index()
        {
            var bankManagementDbContext = _context.RegisteredPayees.Include(r => r.Customer).Where(m => m.Customer.UserId == HttpContext.Session.GetInt32("UserId")).ToList();
            return View(bankManagementDbContext);

        }
        public async Task<IActionResult> SentMoneyIndex(string searchName)
        {
            var mypayees = _context.RegisteredPayees.Include(r => r.Customer).Where(m => m.Customer.UserId == HttpContext.Session.GetInt32("UserId"));
             if (!string.IsNullOrEmpty(searchName))
            {
                mypayees = mypayees.Where(s => s.Name!.Contains(searchName));
            }
              var Payees = new RegisteredPayeeViewModel
            { 
                RegisteredPayees = await mypayees.ToListAsync()
            };
            return View(Payees);

        }
        [HttpPost]
        public string SentMoneyIndex(string searchName , bool notUsed)
        {
            return "From [HttpPost]SentMoneyIndex: filter on " + searchName;
        }


        // GET: RegisteredPayee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredPayee = await _context.RegisteredPayees
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registeredPayee == null)
            {
                return NotFound();
            }

            return View(registeredPayee);
        }

        // GET: RegisteredPayee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegisteredPayee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisteredPayee registeredPayee)
        {
            if (ModelState.IsValid)
            {
                if (registeredPayee.Bank == "Indian Bank")
                {
                    var registeredPayeeExists = _context.Accounts.FirstOrDefault(a => a.AccountNumber == registeredPayee.AccountNumber);
                    if (registeredPayeeExists != null)
                    {
                        registeredPayee.CustomerId = _context.Customers.FirstOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")).Id;
                        _context.Add(registeredPayee);
                        _context.SaveChanges();
                        return RedirectToAction("Home","Customer");
                    }
                    TempData["msg"] = "<script>alert('Account Number Doesnot Exist');</script>";
                }
                else
                {
                    registeredPayee.CustomerId = _context.Customers.FirstOrDefault(m => m.UserId == HttpContext.Session.GetInt32("UserId")).Id;
                    _context.Add(registeredPayee);
                    _context.SaveChanges();
                    return RedirectToAction("Home","Customer");
                }

            }
            return View(registeredPayee);
        }

        // GET: RegisteredPayee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredPayee = await _context.RegisteredPayees.FindAsync(id);
            if (registeredPayee == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", registeredPayee.CustomerId);
            return View(registeredPayee);
        }

        // POST: RegisteredPayee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Bank,AccountNumber,BranchCity,Name,NickName,CustomerId")] RegisteredPayee registeredPayee)
        {
            if (id != registeredPayee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registeredPayee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisteredPayeeExists(registeredPayee.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Address", registeredPayee.CustomerId);
            return View(registeredPayee);
        }

        // GET: RegisteredPayee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registeredPayee = await _context.RegisteredPayees
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registeredPayee == null)
            {
                return NotFound();
            }

            return View(registeredPayee);
        }

        // POST: RegisteredPayee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registeredPayee = await _context.RegisteredPayees.FindAsync(id);
            _context.RegisteredPayees.Remove(registeredPayee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisteredPayeeExists(int id)
        {
            return _context.RegisteredPayees.Any(e => e.Id == id);
        }
        public IActionResult CashTransfer(int id)
        {
            var registeredpayee = _context.RegisteredPayees.Include(r => r.Customer).FirstOrDefault(m => m.Id == id);
            TempData["registeredpayee"] = registeredpayee.Id;
            CashTransferViewModel model = new CashTransferViewModel();
            model.RegisteredPayee = registeredpayee;
            List<Account> accountlist = _context.Accounts.Where(a => a.Customer.UserId == HttpContext.Session.GetInt32("UserId")).ToList();
            ViewData["AccountList"] = accountlist.Select(x => new SelectListItem { Value = x.AccountNumber.ToString(), Text = x.AccountNumber.ToString() });
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CashTransfer(CashTransferViewModel model)
        {
            if (ModelState.IsValid)
            {
                var regpayee = TempData["registeredPayee"];
                var sender = _context.Accounts.FirstOrDefault(m => m.AccountNumber == Convert.ToInt32(model.SenderAccountNumber.Value));
                var receiver = _context.RegisteredPayees.Include(r => r.Customer).FirstOrDefault(m => m.Id == Convert.ToInt32(TempData["registeredpayee"]));
                if (model.Amount <= sender.Balance)
                {
                    MoneyTransfer moneytransfer= new MoneyTransfer{
                        RegisteredPayeeId= receiver.Id,
                        Amount=model.Amount,
                        SenderAccountNumber=model.SenderAccountNumber,
                    }; 
                    _context.Add(moneytransfer);
                    _context.SaveChanges();
                    TempData["MoneyID"] = moneytransfer.Id;
                    TempData.Keep("MoneyID");
                    return RedirectToAction("Review","MoneyTransfer");
                }
                return View();
            }
            return View();
        }
       
    }
}




