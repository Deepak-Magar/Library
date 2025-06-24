using ELibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Services
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Where(m => !m.isDeleted)
                .ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateMemberAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMemberAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                member.isDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
