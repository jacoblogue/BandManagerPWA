using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroups()
        {
            Log.Information("GetGroups endpoint hit");

            // TODO: If user is admin, get all groups. Otherwise, get only groups that user is a member of.
            var groups = _context.Groups.ToList();

            // Return an empty list if no groups are found
            return Ok(groups ?? []);
        }

        [HttpGet("{id}"), Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            Log.Information("GetGroup endpoint hit");

            var group = _context.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        [HttpPost, Authorize(Policy = "create:groups")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO incomingGroup)
        {
            Log.Information("CreateGroup endpoint hit");
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                Name = incomingGroup.Name,
                Description = incomingGroup.Description,
                Users = []
            };

            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            Log.Information("New group created: {@newGroup}", newGroup);
            return Ok(newGroup);
        }

        [HttpDelete("{id}"), Authorize(Policy = "delete:groups")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            Log.Information("DeleteGroup endpoint hit");

            var group = _context.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            Log.Information("Group deleted: {@Group}", group);
            return Ok();
        }

        [HttpPut("{id}/info"), Authorize(Policy = "update:groups")]
        public async Task<IActionResult> UpdateGroupInfo(Guid id, GroupDTO updatedGroup)
        {
            Log.Information("UpdateGroupInfo endpoint hit");
            
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = updatedGroup.Name;
            group.Description = updatedGroup.Description;
            await _context.SaveChangesAsync();

            bool anyChanges = _context.ChangeTracker.Entries().Any(e => e.State == EntityState.Modified);

            if (anyChanges)
                Log.Information("Group updated: {@Group}", group);

            return Ok(group);
        }

        [HttpPost("{groupId}/members")]
        public async Task<IActionResult> AddMember(Guid groupId, string userEmail)
        {
            Log.Information("AddMember endpoint hit");

            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            group.Users.Add(user);
            await _context.SaveChangesAsync();

            Log.Information("User added to group: {@User}", user);
            return Ok();
        }

        [HttpDelete("{groupId}/members/{userId}")]
        public async Task<IActionResult> RemoveMember(Guid groupId, Guid userId)
        {
            Log.Information("RemoveMember endpoint hit");

            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            group.Users.Remove(user);
            await _context.SaveChangesAsync();

            Log.Information("User removed from group: {@User}", user);
            return Ok();
        }
    }
}
