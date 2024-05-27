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
    public class ArtistController : ControllerBase
    {
        private IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet, Authorize(Policy = "read:artists")]
        public async Task<IActionResult> GetArtists()
        {
            try
            {
                Log.Information("GetArtists endpoint hit");

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                List<Artist> artists = await _artistService.GetAllArtistsAsync();

                List<ArtistDTO> artistDTOs = ArtistDtoTransformer.TransformToDtoList(artists);

                return Ok(artistDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Authorize(Policy = "write:artists")]
        public async Task<IActionResult> AddArtist([FromBody] ArtistDTO artistDTO)
        {
            try
            {
                Log.Information("AddArtist endpoint hit");

                if (User is null)
                {
                    Log.Warning("User is null");
                    return BadRequest();
                }

                var artist = await _artistService.GetArtistByNameAsync(artistDTO.Name);

                if (artist is null)
                {
                    await _artistService.CreateArtistAsync(artistDTO);

                    return Ok();
                }
                else
                {
                    return BadRequest("Artist already exists");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
