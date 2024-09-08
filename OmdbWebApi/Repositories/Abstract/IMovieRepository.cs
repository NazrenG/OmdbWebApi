using OmdbWebApi.Entities;

namespace OmdbWebApi.Repositories.Abstract
{
    public interface IMovieRepository
    {
        public Task<IEnumerable<Movie>> GetAllAsync();
        public Task<Movie> GetAsync(int id);

        public Task Add(Movie movie);
        public Task Update(Movie movie);
        public Task Delete(int id);
    }
}
