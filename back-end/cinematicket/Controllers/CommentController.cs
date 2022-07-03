using System.Collections.Generic;
using cinematicket.Models;
using Microsoft.AspNetCore.Mvc;
using cinematicket.Models.Interfaces;

namespace cinematicket.Controllers
{
    [ApiController]
    [Route("comment/")]
    public class CommentController : Controller
    {
        private readonly ISqlHandler _sqlHandler;
        public CommentController(ISqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }
        [HttpGet]
        [Route("getComments")]
        public IActionResult GetFilmDescription([FromQuery] string title)
        {
            var comments = new List<Comment>();
            try
            {
                var query = $"select * from comments where movie_id = '{title}'";
                var movieExist = _sqlHandler.RecordExists(query);
                if (!movieExist)
                {
                    return BadRequest("movie does not exist!");
                }
                var reader = _sqlHandler.ExecuteReader(query);
                while (reader.Read())
                {
                    var comment = new Comment
                    {
                        Username = reader["user_id"].ToString(),
                        FilmTitle = reader["movie_id"].ToString(),
                        Text = reader["text"].ToString()
                    };
                    comments.Add(comment);
                }

                return Ok(comments);

            }
            catch
            {
                return BadRequest("An error occurred");
            }
        }
        
        [HttpPost]
        [Route("addComment")]
        public IActionResult AddComment([FromBody] Comment comment)
        {
            try
            {
                var insertQuery = $"insert into comments values ('{comment.Username}', '{comment.FilmTitle}', '{comment.Text}')";
                _sqlHandler.ExecuteNonQuery(insertQuery);
                return Ok("Comment added successfully");
            }
            catch
            {
                return BadRequest("Add comment failed");
            }
        }
    }
}