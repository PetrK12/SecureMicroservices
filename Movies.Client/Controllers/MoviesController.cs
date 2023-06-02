using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.ApiServices;
using Movies.Client.Models;

namespace Movies.Client.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieApiService _movieApiService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieApiService movieApiService, ILogger<MoviesController> logger)
        {
            _movieApiService = movieApiService;
            _logger = logger;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();
            var movies = await _movieApiService.GetMovies();
            return movies.Any() ? View(movies) : Problem("Movies are empty");
        }
        
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movies = await _movieApiService.GetMovies();
            if (id <= 0 || !movies.Any())
            {
                return NotFound();
            }

            var movie = await _movieApiService.GetMovie(id);
            
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public async Task LogTokenAndClaims()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            
            _logger.LogDebug($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                _logger.LogDebug($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rating,Owner,ReleaseDate,Title,Genre,ImageUrl")] Movie movie)
        {
            if (!ModelState.IsValid) return View(movie);
            await _movieApiService.CreateMovie(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movies = await _movieApiService.GetMovies();
            if (id < 0 || !movies.Any())
            {
                return NotFound();
            }

            var movie = await _movieApiService.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,Owner,ReleaseDate,Title,Genre,ImageUrl")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieApiService.UpdateMovie(movie);
                }
                catch (Exception ex)
                {
                    if (!await MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movies = await _movieApiService.GetMovies();
            if (id < 0 || !movies.Any())
            {
                return NotFound();
            }

            var movie = await _movieApiService.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movies = await _movieApiService.GetMovies();
            if (!movies.Any())
            {
                return Problem("Entity set 'MoviesClientContext.Movie'  is null.");
            }

            var movie = movies.FirstOrDefault(x => x.Id == id);
            if (movie != null)
            {
                await _movieApiService.DeleteMovie(movie.Id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MovieExists(int id)
        { 
          var movies = await _movieApiService.GetMovies();
          return (movies.Any(e => e.Id == id));
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
