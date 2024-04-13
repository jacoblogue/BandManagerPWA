﻿using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.Services.Services
{
    public class SongService : ISongService
    {
        private ApplicationDbContext _context;
        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Song?> GetSongByIdAsync(Guid id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _context.Songs.ToListAsync() ?? [];
        }

        public async Task<List<Song>> GetSongsByArtstIdAsync(Guid artistId)
        {
            return await _context.Songs.Where(s => s.Artist.Id == artistId).ToListAsync() ?? [];
        }
    }
}
