using ELibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooksAsync(string searchString, int page, int pageSize)
        {
            var query = _context.Books.Where(b => !b.isDeleted);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(b => b.Title.Contains(searchString));
            }

            var books = await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var borrowedIds = await _context.Transactions
                .Where(t => t.ReturnDate == null)
                .Select(t => t.BookId)
                .ToListAsync();

            books.ForEach(book => book.IsAvailable = !borrowedIds.Contains(book.Id));

            return books;
        }

        public async Task<int> GetTotalBookCountAsync(string searchString)
        {
            var query = _context.Books.Where(b => !b.isDeleted);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(b => b.Title.Contains(searchString));
            }

            return await query.CountAsync();
        }

        public async Task<List<Book>> GetFilteredBooksAsync(string? searchString)
        {
            var query = _context.Books.Where(b => !b.isDeleted);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(b => b.Title.Contains(searchString));
            }

            var books = await query.ToListAsync();

            var borrowedIds = await _context.Transactions
                .Where(t => t.ReturnDate == null)
                .Select(t => t.BookId)
                .ToListAsync();

            books.ForEach(book => book.IsAvailable = !borrowedIds.Contains(book.Id));

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.isDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
