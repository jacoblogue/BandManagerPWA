using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Services.Interfaces
{
    /// <summary>
    /// Represents an interface for managing groups.
    /// </summary>
    public interface IGroupService
    {
        Task<Group> AddMemberAsync(Guid groupId, string userEmail);

        /// <summary>
        /// Creates a new group asynchronously.
        /// </summary>
        /// <param name="newGroup">The new group to create.</param>
        /// <returns>A task representing the asynchronous operation. The created group.</returns>
        Task<Group> CreateGroupAsync(Group newGroup);
        Task<Group?> DeleteGroupAsync(Guid id);

        /// <summary>
        /// Retrieves all groups asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. A list of groups.</returns>
        Task<List<Group>> GetAllGroupsAsync();

        /// <summary>
        /// Retrieves a group by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the group to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The retrieved group, or null if not found.</returns>
        Task<Group?> GetGroupByIdAsync(Guid id);

        /// <summary>
        /// Retrieves groups associated with the specified email asynchronously.
        /// </summary>
        /// <param name="email">The email address to retrieve groups for.</param>
        /// <returns>A task representing the asynchronous operation. A list of groups.</returns>
        Task<List<Group>> GetGroupsByEmailAsync(string email);
        Task<Group> RemoveMemberAsync(Guid groupId, Guid userId);
        Task<Group> UpdateGroupInfoAsync(Guid id, GroupDTO updatedGroup);
    }
}
