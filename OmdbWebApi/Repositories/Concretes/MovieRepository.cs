using Microsoft.EntityFrameworkCore;
using OmdbWebApi.Data;
using OmdbWebApi.Entities;
using OmdbWebApi.Repositories.Abstract;

namespace OmdbWebApi.Repositories.Concretes
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _db;

        public MovieRepository(MovieDbContext db)
        {
            _db = db;
        }

        public async Task Add(Movie movie)
        {
            await _db.AddAsync(movie);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = await _db.Movies.FirstOrDefaultAsync(x => x.Id == id);
          await  Task.Run(() =>
            {
                _db.Movies.Remove(item);
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                return _db.Movies;
            });
        }

        public async Task<Movie> GetAsync(int id)
        {
            return await _db.Movies.FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task Update(Movie movie)
        {
            await Task.Run(() =>
            {
                _db.Movies.Update(movie);
            });
        }
    }
}
