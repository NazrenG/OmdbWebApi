using OmdbWebApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OmdbWebApi.Services.Concretes
{
    public class FetchMovieService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public FetchMovieService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // This method is invoked when the background service starts
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Create a scoped service to access MovieService and MovieDbContext
                    var movieService = scope.ServiceProvider.GetRequiredService<MovieService>();
                    var movieDbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

                    // Fetch JSON data from the API
                    var jsonData = await movieService.GetJsonDataFromUrl();

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        // Parse the fetched data and store it in the database
                        var movies = movieService.ParseJsonData(jsonData);
                        await movieService.StoreMoviesData(movies);
                    }

                    // Optional: This line may not be necessary if you are saving inside `StoreMoviesData`
                    await movieDbContext.SaveChangesAsync();
                }

                // Wait for 15 minutes before fetching again
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
