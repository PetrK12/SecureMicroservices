using Movies.Client.Models;

namespace Movies.Client.ApiServices;

public class MovieAppService : IMovieApiService
{
    
    public async Task<IEnumerable<Movie>> GetMovies()
    {
        var movieList = new List<Movie>
        {
            new Movie
            {
             Id   = 1,
             Genre = "Thriller",
             Owner = "petr",
             Rating = "9.2",
             Title = "Scream",
             ImageUrl = "www.image.com",
             ReleaseDate = DateTime.Now
            }
        };
        return await Task.FromResult(movieList);
    }

    public Task<Movie> GetMovie(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> CreateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> UpdateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMovie(int id)
    {
        throw new NotImplementedException();
    }
}