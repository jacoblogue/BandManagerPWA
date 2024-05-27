using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Services.Interfaces
{
    public interface IArtistService
    {
        Task<Artist> CreateArtistAsync(ArtistDTO artistDTO);
        Task<List<Artist>> GetAllArtistsAsync();
        Task<Artist?> GetArtistByIdAsync(Guid id);
        Task<Artist?> GetArtistByNameAsync(string name);
    }
}