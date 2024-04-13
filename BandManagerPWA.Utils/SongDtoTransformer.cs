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

        // TODO: Implement TransformToSong method. Get the Artist name using ArtistService.
        //public static Song TransformToSong(SongDTO songDto)
        //{
        //    return new Song
        //    {
        //        Id = songDto.Id,
        //        Title = songDto.Title,
        //        Artist = songDto.Artist,
        //    };
        //}

        public static List<SongDTO> TransformToDtoList(List<Song> songs)
        {
            return songs.Select(TransformToDto).ToList();
        }

        //public static List<Song> TransformToSongList(List<SongDTO> songDtos)
        //{
        //    return songDtos.Select(TransformToSong).ToList();
        //}
    }
}
