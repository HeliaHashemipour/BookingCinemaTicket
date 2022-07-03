using System;
using cinematicket.Models;
using cinematicket.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cinematicket.Controllers
{
    [ApiController]
    [Route("admin/")]
    public class AdminController : Controller
    {
        
        private readonly ISqlHandler _sqlHandler;
        public AdminController(ISqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }
        
        [HttpPost]
        [Route("addFilm")]
        public IActionResult AddFilm([FromBody] Film film)
        {
            try
            {
                var query = $"select * from movies where title = '{film.Title}'";
                var movieExists = _sqlHandler.RecordExists(query);
                if (movieExists)
                {
                    return BadRequest("Film already exist"); 
                }

                var title = film.Title;
                var actors = string.Join( ",", film.Actors.ToArray());
                var director = film.Director;
                var description = film.Description;
                var release = film.Release;
                var duration = film.Duration;
                var rate = film.Rate;
                var trailer = film.Trailer;
                var insertQuery =
                    $"insert into movies values ('{title}', '{actors}', '{director}', '{description}', '{release}', '{duration}', '{rate}', '{trailer}')";
                _sqlHandler.ExecuteNonQuery(insertQuery);
                return Ok("film added successfully");
            }
            catch
            {
                return BadRequest("Add film failed");
                
            }
        }
    }
}