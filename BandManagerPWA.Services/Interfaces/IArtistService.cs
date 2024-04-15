using BandManagerPWA.DataAccess.Models;

namespace BandManagerPWA.Services.Interfaces
{
    public interface IArtistService
    {
        Task<List<Artist>> GetAllArtistsAsync();
        Task<Artist?> GetArtistByIdAsync(Guid id);
        Task<Artist?> GetArtistByNameAsync(string name);
    }
}