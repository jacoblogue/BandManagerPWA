using BandManagerPWA.DataAccess.Models;
using BandManagerPWA.Utils.Models;

namespace BandManagerPWA.Utils
{
    public class SongDtoTransformer
    {
        public static SongDTO TransformToDto(Song song)
        {
            return new SongDTO
            {
                Id = song.Id,
                Title = song.Title,
                Artist = song.Artist.Name,
                Key = song.Key,
            };
        }

        public static Song TransformToSong(SongDTO songDto, Artist artist)
        {

            return new Song
            {
                Id = songDto.Id,
                Title = songDto.Title,
                Artist = artist,
            };
        }

        public static List<SongDTO> TransformToDtoList(List<Song> songs)
        {
            return songs.Select(TransformToDto).ToList();
        }

        //TODO: Implement TransformToSongList
    }
}
