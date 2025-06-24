using ELibrary.Models;
using ELibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return View(transactions);
        }

        public IActionResult Create()
        {
            _transactionService.LoadDropdownData(ViewData);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.CreateTransactionAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            _transactionService.LoadDropdownData(ViewData, transaction.BookId, transaction.MemberId);
            return View(transaction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _transactionService.GetTransactionByIdAsync(id.Value);
            if (transaction == null) return NotFound();

            _transactionService.LoadDropdownData(ViewData, transaction.BookId, transaction.MemberId, transaction.Id);
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                await _transactionService.UpdateTransactionAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            _transactionService.LoadDropdownData(ViewData, transaction.BookId, transaction.MemberId, transaction.Id);
            return View(transaction);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _transactionService.GetTransactionByIdAsync(id.Value);
            if (transaction == null) return NotFound();

            return View(transaction);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _transactionService.GetTransactionByIdAsync(id.Value);
            if (transaction == null) return NotFound();

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _transactionService.SoftDeleteTransactionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
