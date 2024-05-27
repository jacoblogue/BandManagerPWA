using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils.DtoTransformers;
using BandManagerPWA.Utils.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private ISongService _songService;
        private IArtistService _artistService;
        public SongController(ISongService songService, IArtistService artistService)
        {
            _songService = songService;
            _artistService = artistService;
        }

        [HttpGet, Authorize(Policy = "read:songs")]
        public async Task<IActionResult> GetSongs()
        {
            try
            {
                Log.Information("GetSongs endpoint hit");

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                List<Song> songs = await _songService.GetAllSongsAsync();

                List<SongDTO> songDTOs= SongDtoTransformer.TransformToDtoList(songs);

                return Ok(songDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Authorize(Policy = "write:songs")]
        public async Task<IActionResult> CreateSong([FromBody] SongDTO songDTO)
        {
            try
            {
                Log.Information("AddSong endpoint hit");

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                var songArtist = await _artistService.GetArtistByNameAsync(songDTO.Artist);

                if (songArtist is null)
                {
                    Log.Warning("Artist not found");
                    return NotFound("Artist not found");
                }

                Song song = SongDtoTransformer.TransformToSong(songDTO, songArtist);
                await _songService.AddSongAsync(song);
                Log.Information($"Song added: {song.Title} by {song.Artist}");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
