using System;
using System.Linq;
using cinematicket.Models;
using cinematicket.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cinematicket.Controllers
{
    [ApiController]
    [Route("film/")]
    public class FilmController : Controller
    {
        private readonly ISqlHandler _sqlHandler;
        public FilmController(ISqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }
        
        [HttpGet]
        [Route("getFilm")]
        public IActionResult GetFilmDescription([FromQuery] string title)
        {
            try
            {
                var query = $"select * from movies where title = '{title}'";
                var movieExists = _sqlHandler.RecordExists(query);
                Console.WriteLine(query);
                if (!movieExists)
                {
                    return BadRequest("movie does not exist!"); 
                }
                var selectedMovie = _sqlHandler.ExecuteReader(query);
                var film = new Film
                {
                    Title = selectedMovie["title"].ToString(),
                    Actors = selectedMovie["actors_names"].ToString()?.Split(",").ToList(),
                    Director = selectedMovie["director"].ToString(),
                    Description = selectedMovie["description"].ToString(),
                    Release = (int) selectedMovie["release_year"],
                    Duration = (int) selectedMovie["duration"],
                    Rate = (float) selectedMovie["rating"],
                    Trailer = selectedMovie["trailer"].ToString()
                };
                return Ok(film);
            }
            catch
            {
                return BadRequest("An error occurred");
            }
        }
    }
}