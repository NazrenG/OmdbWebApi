using OmdbWebApi.Data;
using OmdbWebApi.Entities;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.EntityFrameworkCore;

namespace OmdbWebApi.Services
{
    public class FetchMovieService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public FetchMovieService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var movieDb = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

                    // Random herf secilir
                    int asciiNumber =Random.Shared.Next(0,26);
                    string selectedLetter = ((char)(asciiNumber+65)).ToString();

                    // API'den data cekilir
                    var response = await GetDataFromUrlAsync(selectedLetter);

                    // Response'un boş olub olmadigini yoxlamq
                    if (!string.IsNullOrEmpty(response))
                    {
                        var movieList = ParseJsonToMovie(response, selectedLetter);

                        foreach (var movie in movieList)
                        {
                            // Eger db-de varsa yazilmasin
                            bool exists = await movieDb.Movies.AnyAsync(m => m.Id == movie.Id);
                            if (!exists)
                            {
                                movieDb.Movies.Add(movie);
                            }
                        }

                        await movieDb.SaveChangesAsync();
                    }
                }

                // 15 san
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        // API'ye istek gönderme
        public async Task<string> GetDataFromUrlAsync(string selectedLetter)
        {
            var options = new RestClientOptions("https://api.themoviedb.org/3/search/movie");
            var client = new RestClient(options);

            var request = new RestRequest();
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxYTU3NmQ4N2VhOThiODMyNDQxYjVhYjIwZGE4NTNkYiIsIm5iZiI6MTcyNjE1NzU0MS4yODc0NjYsInN1YiI6IjY2OGQxNTU0NTRkMTg5ZmUzMTRhZWNhYSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.JJn1JvvyM1EyJnD2o1Tb4_O2iiltl27abKfQ6SlTFas");

            request.AddQueryParameter("query", selectedLetter);
            var response = await client.GetAsync(request);

            return response.Content;
        }

        // JSON datasini parse etme ve Movie obyektini cevirme
        public List<Movie> ParseJsonToMovie(string response, string selectedLetter)
        {
            JObject jsonObject = JObject.Parse(response);
            JArray results = (JArray)jsonObject["results"];
            var movieList = new List<Movie>();

            foreach (var item in results)
            {

                if (item["title"].ToString().StartsWith(selectedLetter, StringComparison.OrdinalIgnoreCase))
                {
                    var movie = new Movie
                    {
                        PhotoUrl = item["poster_path"]?.ToString(),
                        Title = item["title"]?.ToString(),
                        Description = item["overview"]?.ToString()
                    };

                    movieList.Add(movie);
                }
            }

            return movieList;
        }
    }
}
