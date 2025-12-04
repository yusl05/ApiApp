using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RawGames.Model
{
    public class Response
    {
        [JsonPropertyName("album")]
        public List<Game> Results { get; set; } = new();

    }

    public class Game
    {
        [JsonPropertyName("idAlbum")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("strAlbum")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("strAlbumThumb")]
        public string BackgroundImage { get; set; } = string.Empty;

        [JsonPropertyName("strArtist")]
        public string Artist { get; set; } = string.Empty;

        [JsonPropertyName("strGenre")]
        public string Genre { get; set; } = string.Empty;

        public string MetacriticFormatted =>
            !string.IsNullOrEmpty(Genre) ? $"Género: {Genre}" : "Sin género";



    }

    public class GameDetail : Game
    {
        [JsonPropertyName("strDescriptionEN")]
        public string Description { get; set; } = string.Empty;
    }
}   
