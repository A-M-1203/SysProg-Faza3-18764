using System.Reactive.Linq;
using SysProg_faza3;
using SysProg_faza3.Services;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server();
        _ = Task.Run(() => server.Start(8080));

        Console.ReadLine();
        //YelpService yelpService = new YelpService(apiKey);
        //var response = await yelpService.GetRestaurantsAsync(location);

        //var restaurantsObservable = response.Businesses.ToObservable();
        //var filteredRestaurants = YelpService.FilterAndSort(restaurantsObservable);

        //filteredRestaurants.Subscribe(
        //    restaurant => Console.WriteLine($"Naziv: {restaurant.Name}, Ocena: {restaurant.Rating}, Broj ocena: {restaurant.ReviewCount}, Cena: {restaurant.Price}"),
        //    ex => Console.WriteLine($"ERROR: {ex.Message}"),
        //    () => Console.WriteLine("Completed")
        //);
    }
}
