using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Utils.DtoTransformers
{
    public class ArtistDtoTransformer
    {
        public static ArtistDTO TransformToDto(Artist artist)
        {
            return new ArtistDTO
            {
                Name = artist.Name,
            };
        }

        public static Artist TransformToArtist(ArtistDTO artistDto)
        {
            return new Artist
            {
                Name = artistDto.Name,
            };
        }

        public static List<ArtistDTO> TransformToDtoList(List<Artist> artists)
        {
            return artists.Select(TransformToDto).ToList();
        }

        public static List<Artist> TransformToArtistList(List<ArtistDTO> artistDtos)
        {
            return artistDtos.Select(TransformToArtist).ToList();
        }
    }
}
