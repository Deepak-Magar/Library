using ELibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class TransactionController : Controller
{
    private readonly AppDbContext _context;

    public TransactionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var transactions = await _context.Transactions
    .Where(t => !t.IsDeleted)
    .Include(t => t.Book)
    .Include(t => t.Member)
    .ToListAsync();
        return View(transactions);
    }

    public IActionResult Create()
    {
        LoadDropdownData();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            var book = await _context.Books.FindAsync(transaction.BookId);
            var member = await _context.Members.FindAsync(transaction.MemberId);

            if (book != null) transaction.BookTitle = book.Title;
            if (member != null) transaction.MemberName = member.FullName;

            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        LoadDropdownData(transaction.BookId, transaction.MemberId);
        return View(transaction);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();

        LoadDropdownData(transaction.BookId, transaction.MemberId, transaction.Id);
        return View(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Transaction transaction)
    {
       
        if (ModelState.IsValid)
        {
                var book = await _context.Books.FindAsync(transaction.BookId);

                var member = await _context.Members.FindAsync(transaction.MemberId);

                if (book != null) transaction.BookTitle = book.Title;
                if (member != null) transaction.MemberName = member.FullName;

                _context.Update(transaction);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
        }

        LoadDropdownData(transaction.BookId, transaction.MemberId, transaction.Id);
        return View(transaction);
    }

    public async Task<IActionResult> Delete(int? id)
{
    if (id == null) return NotFound();

    var transaction = await _context.Transactions
        .Include(t => t.Book)
        .Include(t => t.Member)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (transaction == null) return NotFound();

    return View(transaction);
}

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        transaction.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransactionExists(int id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }

    private void LoadDropdownData(int? selectedBookId = null, int? selectedMemberId = null, int? editingTransactionId = null)
    {
        var borrowedBookIds = _context.Transactions
            .Where(t => t.ReturnDate == null && t.Id != editingTransactionId)
            .Select(t => t.BookId)
            .ToList();

        var availableBooks = _context.Books
            .Where(b => !b.isDeleted && (!borrowedBookIds.Contains(b.Id) || b.Id == selectedBookId))
            .ToList();

        var activeMembers = _context.Members
            .Where(m => !m.isDeleted)
            .ToList();

        ViewBag.BookList = new SelectList(availableBooks, "Id", "Title", selectedBookId);
        ViewBag.MemberList = new SelectList(activeMembers, "Id", "FullName", selectedMemberId);
    }

}
