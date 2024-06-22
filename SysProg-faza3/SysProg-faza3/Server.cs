using System.Net;
using System.Web;
using System.Text;
using SysProg_faza3.Services;
using System.Reactive.Linq;

namespace SysProg_faza3;

public class Server
{
    private static readonly HttpListener _listener = new HttpListener();

    public async Task Start(int port)
    {
        _listener.Prefixes.Add($"http://localhost:{port}/");

        _listener.Start();
        Console.WriteLine($"Listening for incoming requests on port {port}...");

        while (true)
        {
            // Wait for an incoming request
            HttpListenerContext context = await _listener.GetContextAsync();
            HttpListenerRequest request = context.Request;

            // Parse the city name from the URL
            string? cityName = GetCityNameFromUrl(request.Url!);

            // Write the city name to the console
            //Console.WriteLine($"City: {cityName}");

            // Respond to the request
            HandleResponse(context, cityName);
            if (cityName != null)
            {
                await CallService(cityName);
            }
        }
    }

    private string? GetCityNameFromUrl(Uri url)
    {
        // npr. http://localhost:8080/?city=NewYork
        var query = url.Query;
        var queryDictionary = HttpUtility.ParseQueryString(query);
        return queryDictionary["city"] ?? null;
    }

    private void HandleResponse(HttpListenerContext context, string? cityName)
    {
        HttpListenerResponse response = context.Response;
        string responseString = $"<html><body>Received city: {cityName}</body></html>";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        output.Close();
    }

    private async Task CallService(string location)
    {
        string apiKey = "wRTZ6q0m0KxlJ9rYHMXkE01zZYQ7yOpzHhsRmIV8V-dVZ4fx3b-BrKH7PCtt_tT1_SqlhQOJ07HEcqADcBYMuHzaDindH4Yd4XmJWKT6vjs23RtR9SPuSr-Tqhx3ZnYx";

        var response = await YelpService.GetRestaurantsAsync(location, apiKey);

        var restaurantsObservable = response.Businesses.ToObservable();
        var filteredRestaurants = YelpService.FilterAndSort(restaurantsObservable);

        filteredRestaurants.Subscribe(
            restaurant => Console.WriteLine($"Naziv: {restaurant.Name}, Ocena: {restaurant.Rating}, Broj ocena: {restaurant.ReviewCount}, Cena: {restaurant.Price}"),
            ex => Console.WriteLine($"ERROR: {ex.Message}"),
            () => Console.WriteLine("Completed")
        );
    }
}
