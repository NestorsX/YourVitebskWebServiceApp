using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MyVitebskWebServiceApp.Models;
using System.Threading.Tasks;

namespace MyVitebskWebServiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private MyVitebskDBContext _context;

        public UsersController(MyVitebskDBContext context)
        {
            _context = context;
        }

        // Gets the list of all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // Gets one user by id (api/users/1)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        [HttpGet("auth/{username}/{password}")]
        public async Task<ActionResult<User>> Get(string username, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        // Inserts user into table (api/users)
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // Updates user (api/users)
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!_context.Users.Any(x => x.UserId == user.UserId))
            {
                return NotFound();
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // Deletes user by id
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
