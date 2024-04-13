using BandManagerPWA.DataAccess.Models;

namespace BandManagerPWA.Services.Interfaces
{
    public interface ISongService
    {
        Task<List<Song>> GetAllSongsAsync();
        Task<Song?> GetSongByIdAsync(Guid id);
        Task<List<Song>> GetSongsByArtstIdAsync(Guid artistId);
    }
}