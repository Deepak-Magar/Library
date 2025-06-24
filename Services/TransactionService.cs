using ELibrary.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Where(t => !t.IsDeleted)
                .Include(t => t.Book)
                .Include(t => t.Member)
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Member)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            var book = await _context.Books.FindAsync(transaction.BookId);
            var member = await _context.Members.FindAsync(transaction.MemberId);

            if (book != null) transaction.BookTitle = book.Title;
            if (member != null) transaction.MemberName = member.FullName;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            var book = await _context.Books.FindAsync(transaction.BookId);
            var member = await _context.Members.FindAsync(transaction.MemberId);

            if (book != null) transaction.BookTitle = book.Title;
            if (member != null) transaction.MemberName = member.FullName;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        public void LoadDropdownData(ViewDataDictionary viewData, int? selectedBookId = null, int? selectedMemberId = null, int? editingTransactionId = null)
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

            viewData["BookList"] = new SelectList(availableBooks, "Id", "Title", selectedBookId);
            viewData["MemberList"] = new SelectList(activeMembers, "Id", "FullName", selectedMemberId);
        }
    }
}
