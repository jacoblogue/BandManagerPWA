using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils.DtoTransformers;
using BandManagerPWA.Utils.Models;
using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.Services.Services
{
    public class ArtistService : IArtistService
    {
        private ApplicationDbContext _context;
        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Artist?> GetArtistByIdAsync(Guid id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<Artist?> GetArtistByNameAsync(string name)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase));
        }


        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            return await _context.Artists.ToListAsync() ?? [];
        }

        public async Task<Artist> CreateArtistAsync(ArtistDTO artistDTO)
        {
            var newArtist = ArtistDtoTransformer.TransformToArtist(artistDTO);

            await _context.Artists.AddAsync(newArtist);
            await _context.SaveChangesAsync();
            return newArtist;
        }
    }
}
