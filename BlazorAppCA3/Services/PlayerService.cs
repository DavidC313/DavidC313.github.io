using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using X00181967CA3.Models;

public static class PlayerService
{
    private const string ApiHost = "free-api-live-football-data.p.rapidapi.com";
    private const string ApiKey = "3e90be2708mshe9759b0e36f046bp12d1d5jsn631f165d5dc4";

    public static async Task<List<Player>> SearchPlayersAsync(HttpClient httpClient, string query)
    {
        var url = $"https://{ApiHost}/football-players-search?search={query}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Rapidapi-Key", ApiKey);
        request.Headers.Add("X-Rapidapi-Host", ApiHost);

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"HTTP error {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();
        var parsedResponse = JsonSerializer.Deserialize<JsonElement>(json);

        var players = new List<Player>();
        if (parsedResponse.TryGetProperty("response", out var responseObj) &&
            responseObj.TryGetProperty("suggestions", out var suggestions))
        {
            foreach (var suggestion in suggestions.EnumerateArray())
            {
                var player = new Player
                {
                    Name = suggestion.GetProperty("name").GetString() ?? "Unknown",
                    Team = suggestion.GetProperty("teamName").GetString() ?? "Unknown",
                    Popularity = suggestion.GetProperty("score").GetInt32().ToString(),
                    ImageUrl = "https://via.placeholder.com/50"
                };
                players.Add(player);
            }
        }

        return players;
    }
}
