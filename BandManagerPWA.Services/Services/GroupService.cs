using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BandManagerPWA.Services.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext _context;
        public GroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group?> GetGroupByIdAsync(Guid id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups.ToListAsync() ?? [];
        }

        public async Task<List<Group>> GetGroupsByEmailAsync(string email)
        {
            var groups = await _context.Groups.Where(g => g.Users.Any(u => u.Email == email)).ToListAsync();
            return groups ?? [];
        }

        public async Task<Group> CreateGroupAsync(Group newGroup)
        {
            _context.Groups.Add(newGroup);
            await _context.SaveChangesAsync();
            Log.Information("New group created: {@newGroup}", newGroup);
            return newGroup;
        }

        public async Task<Group> UpdateGroupInfoAsync(Guid id, GroupDTO updatedGroup)
        {
            var existingGroup = await _context.Groups.FindAsync(id);

            if (existingGroup is null)
            {
                throw new Exception("Group not found");
            }

            existingGroup.Name = updatedGroup.Name;
            existingGroup.Description = updatedGroup.Description;

            await _context.SaveChangesAsync();

            bool anyChanges = _context.ChangeTracker.Entries().Any(e => e.State == EntityState.Modified);

            if (anyChanges)
                Log.Information("Group updated: {@Group}", existingGroup);

            return existingGroup;
        }

        public async Task<Group?> DeleteGroupAsync(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group is null)
            {
                return null;
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            Log.Information("Group deleted: {@Group}", group);
            return group;
        }

        public async Task<Group> AddMemberAsync(Guid groupId, string userEmail)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group is null)
            {
                throw new Exception("Group not found");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            group.Users.Add(user);
            await _context.SaveChangesAsync();
            Log.Information("User added to group: {@User}", user);
            return group;
        }

        public async Task<Group> RemoveMemberAsync(Guid groupId, Guid userId)
        {
            var group = await _context.Groups.FindAsync(groupId);

            if (group is null)
            {
                throw new Exception("Group not found");
            }

            var user = await _context.Users.FindAsync(userId);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            group.Users.Remove(user);
            await _context.SaveChangesAsync();
            Log.Information("User removed from group: {@User}", user);
            return group;
        }
    }
}
