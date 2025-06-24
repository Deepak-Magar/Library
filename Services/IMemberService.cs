using ELibrary.Models;

namespace ELibrary.Services
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        Task<Member?> GetMemberByIdAsync(int id);
        Task CreateMemberAsync(Member member);
        Task UpdateMemberAsync(Member member);
        Task SoftDeleteMemberAsync(int id);
    }
}
