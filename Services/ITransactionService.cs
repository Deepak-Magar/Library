using ELibrary.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ELibrary.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task CreateTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task SoftDeleteTransactionAsync(int id);
        bool TransactionExists(int id);
        void LoadDropdownData(ViewDataDictionary viewData, int? selectedBookId = null, int? selectedMemberId = null, int? editingTransactionId = null);
    }
}
