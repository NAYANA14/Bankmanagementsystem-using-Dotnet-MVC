#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankManagement.Models;

namespace bankmanagementsystem.Controllers
{
    public class MoneyTransferController : Controller
    {
        private readonly BankManagementDbContext _context;

        public MoneyTransferController(BankManagementDbContext context)
        {
            _context = context;
        }

        // GET: MoneyTransfer
        public async Task<IActionResult> Index()
        {
            var bankManagementDbContext = _context.MoneyTransfers.Include(m => m.RegisteredPayee);
            return View(await bankManagementDbContext.ToListAsync());
        }

        // GET: MoneyTransfer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneyTransfer = await _context.MoneyTransfers
                .Include(m => m.RegisteredPayee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moneyTransfer == null)
            {
                return NotFound();
            }

            return View(moneyTransfer);
        }

        // GET: MoneyTransfer/Create
        public IActionResult Create()
        {
            ViewData["RegisteredPayeeId"] = new SelectList(_context.RegisteredPayees, "Id", "Bank");
            return View();
        }

        // POST: MoneyTransfer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,SenderAccountNumber,RegisteredPayeeId")] MoneyTransfer moneyTransfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moneyTransfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegisteredPayeeId"] = new SelectList(_context.RegisteredPayees, "Id", "Bank", moneyTransfer.RegisteredPayeeId);
            return View(moneyTransfer);
        }

        // GET: MoneyTransfer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneyTransfer = await _context.MoneyTransfers.FindAsync(id);
            if (moneyTransfer == null)
            {
                return NotFound();
            }
            ViewData["RegisteredPayeeId"] = new SelectList(_context.RegisteredPayees, "Id", "Bank", moneyTransfer.RegisteredPayeeId);
            return View(moneyTransfer);
        }

        // POST: MoneyTransfer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Amount,SenderAccountNumber,RegisteredPayeeId")] MoneyTransfer moneyTransfer)
        {
            if (id != moneyTransfer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moneyTransfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoneyTransferExists(moneyTransfer.Id))
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
            ViewData["RegisteredPayeeId"] = new SelectList(_context.RegisteredPayees, "Id", "Bank", moneyTransfer.RegisteredPayeeId);
            return View(moneyTransfer);
        }

        // GET: MoneyTransfer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moneyTransfer = await _context.MoneyTransfers
                .Include(m => m.RegisteredPayee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moneyTransfer == null)
            {
                return NotFound();
            }

            return View(moneyTransfer);
        }

        // POST: MoneyTransfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var moneyTransfer = await _context.MoneyTransfers.FindAsync(id);
            _context.MoneyTransfers.Remove(moneyTransfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoneyTransferExists(int? id)
        {
            return _context.MoneyTransfers.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Review()
        {
            var transfer = await _context.MoneyTransfers.Include(r => r.RegisteredPayee).FirstOrDefaultAsync(m => m.Id == Convert.ToInt32(TempData["MoneyID"]));
            if (transfer == null)
            {
                return NotFound();
            }
            return View(transfer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Review(int id, MoneyTransfer moneyTransfer)
        {
            var model = _context.MoneyTransfers.Include(r => r.RegisteredPayee).FirstOrDefault(m => m.Id == id);
            Transaction transaction = new Transaction();
            transaction.Date = DateTime.Today;
            transaction.Amount = model.Amount;
            transaction.AccountNumber = model.SenderAccountNumber;
            transaction.Type = "Debit";
            transaction.Account = _context.Accounts.FirstOrDefault(m => m.AccountNumber == model.SenderAccountNumber);
            _context.Add(transaction);
            _context.SaveChanges();
            var account = _context.Accounts.FirstOrDefault(m => m.Id == transaction.AccountId);
            account.Balance = account.Balance - transaction.Amount;
            _context.SaveChanges();
            var bank = _context.RegisteredPayees.FirstOrDefault(m => m.Id == model.RegisteredPayeeId);
            if (bank.Bank == "IndianBank")
            {
                Transaction transaction1 = new Transaction();
                var reciever = _context.Accounts.FirstOrDefault(m => m.AccountNumber == model.RegisteredPayee.AccountNumber);
                transaction1.Amount = model.Amount;
                transaction1.AccountId = reciever.Id;
                transaction1.Date = DateTime.Today;
                transaction1.AccountNumber = model.SenderAccountNumber;
                transaction1.Type = "Credit";
                _context.Add(transaction1);
                _context.SaveChanges();
                var account1 = _context.Accounts.FirstOrDefault(m => m.Id == transaction1.AccountId);
                account1.Balance = account1.Balance + transaction1.Amount;
                _context.SaveChanges();
                }
                return RedirectToAction("Home", "Customer");
            
        }
    }
}
