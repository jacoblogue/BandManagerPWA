using BandManagerPWA.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO incomingUser)
        {
            // idempotent - if user exists, return 200
            // if user doesn't exist, create user and return 201
            // if user exists but is different, update user and return 200
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = incomingUser.Email,
            };

            //_context.Users.Add(user);
            return Ok();
        }
    }
}
