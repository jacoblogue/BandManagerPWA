using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private ISongService _songService;
        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet, Authorize(Policy = "read:songs")]
        public async Task<IActionResult> GetSongs()
        {
            try
            {
                List<Song> songs = await _songService.GetAllSongsAsync();
                return Ok(songs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
