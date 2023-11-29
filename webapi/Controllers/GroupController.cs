using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private ApplicationDbContext _context;

        [HttpGet, Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroups()
        {
            // TODO: If user is admin, get all groups. Otherwise, get only groups that user is a member of.
            var groups = _context.Groups.ToList();

            // Return an empty list if no groups are found
            return Ok(groups ?? []);
        }

        [HttpGet("{id}"), Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
    }
}
