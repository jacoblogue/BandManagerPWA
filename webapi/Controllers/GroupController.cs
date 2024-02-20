using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly string _emailClaimType = "https://bandmanager.com/email";
        private readonly IGroupService _groupService;

        public GroupController(ApplicationDbContext context, IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet, Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                Log.Information("GetGroups endpoint hit");

                List<Group> groups = [];

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                bool readAll = User.HasClaim("permissions", "read:all");

                if (readAll)
                {
                    groups = await _groupService.GetAllGroupsAsync();
                }
                else
                {
                    if (!User.HasClaim(c => c.Type == _emailClaimType) ||
                        string.IsNullOrWhiteSpace(User.Claims.FirstOrDefault(c => c.Type == _emailClaimType)?.Value))
                    {
                        Log.Warning("User email not found");
                        return BadRequest("User email not found");
                    }

                    // get user's email and only get their groups
                    var userEmail = User.Claims.FirstOrDefault(c => c.Type == _emailClaimType)?.Value;
                    groups = await _groupService.GetGroupsByEmailAsync(userEmail);
                }

                // Return an empty list if no groups are found
                return Ok(groups ?? []);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpGet("{id}"), Authorize(Policy = "read:groups")]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            try
            {
                Log.Information("GetGroup endpoint hit");

                var group = await _groupService.GetGroupByIdAsync(id);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpPost, Authorize(Policy = "create:groups")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO incomingGroup)
        {
            try
            {
                Log.Information("CreateGroup endpoint hit");
                var newGroup = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = incomingGroup.Name,
                    Description = incomingGroup.Description,
                    Users = []
                };

                await _groupService.CreateGroupAsync(newGroup);

                return Ok(newGroup);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}"), Authorize(Policy = "delete:groups")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            try
            {
                Log.Information("DeleteGroup endpoint hit");

                var group = await _groupService.DeleteGroupAsync(id);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpPut("{id}/info"), Authorize(Policy = "update:groups")]
        public async Task<IActionResult> UpdateGroupInfo(Guid id, GroupDTO updatedGroup)
        {
            try
            {
                Log.Information("UpdateGroupInfo endpoint hit");

                var result = await _groupService.UpdateGroupInfoAsync(id, updatedGroup);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpPost("{groupId}/members")]
        public async Task<IActionResult> AddMember(Guid groupId, string userEmail)
        {
            try
            {
                Log.Information("AddMember endpoint hit");

                var group = await _groupService.AddMemberAsync(groupId, userEmail);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        [HttpDelete("{groupId}/members/{userId}")]
        public async Task<IActionResult> RemoveMember(Guid groupId, Guid userId)
        {
            try
            {
                Log.Information("RemoveMember endpoint hit");

                var group = await _groupService.RemoveMemberAsync(groupId, userId);

                if (group == null)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }
    }
}
