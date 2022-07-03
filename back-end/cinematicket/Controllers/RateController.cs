using System.Collections.Generic;
using cinematicket.Models;
using Microsoft.AspNetCore.Mvc;
using cinematicket.Models.Interfaces;

namespace cinematicket.Controllers
{
    [ApiController]
    [Route("rate/")]
    public class RateController : Controller
    {
        private readonly ISqlHandler _sqlHandler;
        public RateController(ISqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }
        [HttpGet]
        [Route("getRates")]
        public IActionResult GetFilmDescription([FromQuery] string title)
        {
            var rates = new List<Rate>();
            try
            {
                var query = $"select * from rates where movie_id = {title}";
                var movieExist = _sqlHandler.RecordExists(query);
                if (!movieExist)
                {
                    return BadRequest("movie does not exist!");
                }
                var reader = _sqlHandler.ExecuteReader(query);
                while (reader.Read())
                {
                    var rate = new Rate
                    {
                        Username = reader["user_id"].ToString(),
                        FilmTitle = reader["movie_id"].ToString(),
                        Score = (int) reader["score"]
                    };
                    rates.Add(rate);
                }

                return Ok(rates);

            }
            catch
            {
                return BadRequest("An error occurred");
            }
        }
        
        [HttpPost]
        [Route("addRate")]
        public IActionResult AddComment([FromBody] Rate rate)
        {
            try
            {
                var insertQuery = $"insert into rates values ('{rate.Username}', '{rate.FilmTitle}', '{rate.Score}')";
                _sqlHandler.ExecuteNonQuery(insertQuery);
                return Ok("rate added successfully");
            }
            catch
            {
                return BadRequest("Add rate failed");
            }
        }
    }
}