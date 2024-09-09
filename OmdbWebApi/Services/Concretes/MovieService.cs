using OmdbWebApi.Data;
using OmdbWebApi.Entities;
using System.Text.Json;

namespace OmdbWebApi.Services.Concretes
{
    public class MovieService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly HttpClient _httpClient;

        public MovieService(MovieDbContext movieDbContext, HttpClient httpClient)
        {
            this.movieDbContext = movieDbContext;
            _httpClient = httpClient;
        }

        // api-den JSON data  cekmek
        public async Task<string?> GetJsonDataFromUrl()
        {
            string url = "https://api.themoviedb.org/3/trending/movie/day?api_key=75d57e9716ab196a930bbcff01b2c422";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        //  JSON datani movie obyektine cevirmek
        public List<Movie> ParseJsonData(string jsonData)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(jsonData);
            List<Movie> movies = new List<Movie>();

            //json strukturu
            var results = jsonDoc.RootElement.GetProperty("results");

            foreach (var movieElement in results.EnumerateArray())
            {
                var movie = new Movie
                {
                    Id = movieElement.GetProperty("id").GetInt32(),
                    Title = movieElement.GetProperty("title").GetString(),
                    Description = movieElement.GetProperty("overview").GetString(),
                    PhotoUrl = movieElement.GetProperty("poster_path").GetString(),
                };

                movies.Add(movie);
            }

            return movies;
        }

        // movie obyektini db de saxlamaq
        public async Task StoreMovieData(Movie movie)
        {
            if (movie != null)
            {
                // db-e save etmek
                movieDbContext.Movies.Add(movie);
                await movieDbContext.SaveChangesAsync();
            }
        }

        // movie listini db de saxlamaq
        public async Task StoreMoviesData(List<Movie> movies)
        {
            if (movies != null && movies.Count > 0)
            {
                movieDbContext.Movies.AddRange(movies);
                await movieDbContext.SaveChangesAsync();
            }
        }
    }
}
