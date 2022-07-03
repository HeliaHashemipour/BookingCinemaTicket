using System;
using cinematicket.Models;
using cinematicket.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cinematicket.Controllers
{
    [ApiController]
    [Route("user/")]
    public class UserController : Controller
    {
        private readonly ISqlHandler _sqlHandler;

        public UserController(ISqlHandler sqlHandler)
        {
            _sqlHandler = sqlHandler;
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                var query = $"select count(*) from users where username = '{user.Username}'";
                var userExist = _sqlHandler.RecordExists(query);
                if (userExist)
                {
                    return BadRequest("User already exist");
                }

                var insertQuery = $"insert into users values ('{user.Username}', '{user.Email}', '{user.Password}')";
                _sqlHandler.ExecuteNonQuery(insertQuery);
                return Ok("Registered successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Register failed");
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            try
            {
                var query = $"select count(*) from users where username = '{user.Username}' and pass = '{user.Password}'";
                var userExist = _sqlHandler.RecordExists(query);
                if (userExist)
                {
                    return Ok("Login successful");
                }
                return BadRequest("Username or Password is incorrect!");
            }
            catch
            {
                return BadRequest("Login failed");
            }
        }
    }
}