using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RawGames.Model;
using System.Net.WebSockets;

namespace RawGames.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.theaudiodb.com/api/v1/json"; //posible diagonal al final

        private const string ApiKey = "123";

        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<Response?> GetGamesAsync(int currentPage)
        {
            try
            {
                var url = $"{BaseUrl}/games?key={ApiKey}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Response>(json, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"Error en GamesResponse: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching games: {ex.Message}");
                return null;
            }
        }


        public async Task<Response?> SearchAlbumsAsync(string artist)
        {
            try
            {
                var url = $"{BaseUrl}/{ApiKey}/searchalbum.php?s={Uri.EscapeDataString(artist)}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Response>(json, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"Error en SearchAlbumsAsync: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching albums: {ex.Message}");
                return null;
            }
        }

    }
}