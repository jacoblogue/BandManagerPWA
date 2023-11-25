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
        public async Task<IActionResult> UpsertUser([FromBody] UserDTO incomingUser)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == incomingUser.Email);
                if (existingUser != null)
                {
                    return Ok();
                }
                else
                {
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = incomingUser.Email,
                    };
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetUsers), newUser);
                }
            }
            catch (Exception ex)
            {
                // TODO Add logging etc
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}
