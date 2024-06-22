using SysProg_faza3.Models;
using System.Net.Http.Json;
using System.Reactive.Linq;

namespace SysProg_faza3.Services;

public static class YelpService
{
    private static readonly HttpClient _client = new HttpClient();

    public static async Task<YelpResponse?> GetRestaurantsAsync(string location, string apiKey)
    {
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        var url = $"https://api.yelp.com/v3/businesses/search?location={location}&categories=restaurants";
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<YelpResponse>();
    }

    public static IObservable<RestourantModel> FilterAndSort(IObservable<RestourantModel> source)
    {
        return source
            .Where(r => r.Rating > 4.0 && !r.IsClosed)
            .ToList()
            .SelectMany(businesses =>
                businesses
                .OrderByDescending(b => b.Price?.Length ?? 0)
                .ToObservable()
            );
    }
}
