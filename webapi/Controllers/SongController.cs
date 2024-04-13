using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Services.Interfaces;
using BandManagerPWA.Utils;
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
        public SongController(ISongService songService)
        {
            _songService = songService;
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
    }
}
