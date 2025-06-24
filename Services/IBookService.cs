using ELibrary.Models;

namespace ELibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync(string searchString, int page, int pageSize);
        Task<int> GetTotalBookCountAsync(string searchString);
        Task<List<Book>> GetFilteredBooksAsync(string? searchString);
        Task<Book?> GetBookByIdAsync(int id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task SoftDeleteBookAsync(int id);
        Task<bool> BookExistsAsync(int id);
    }
}
