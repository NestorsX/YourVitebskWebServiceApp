using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly YourVitebskDBContext _context;

        public UsersController(YourVitebskDBContext context)
        {
            _context = context;
        }

        private async Task<UserDatum> GetUserDataForUser(User user)
        {
            return await _context.UserData.FirstOrDefaultAsync(x => x.UserId == user.UserId);
        }

        // Gets the list of all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            foreach (User user in users)
            {
                user.UserDatum = await GetUserDataForUser(user);
            }

            return new ObjectResult(users);
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

            user.UserDatum = await GetUserDataForUser(user);
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

            user.UserDatum = await GetUserDataForUser(user);
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

            UserDatum userDatum = user.UserDatum;
            user.UserId = null;
            user.UserDatum = null;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            userDatum.UserDataId = null;
            userDatum.UserId = (int)user.UserId;
            _context.UserData.Add(userDatum);
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

            _context.Users.Update(user);
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

            user.UserDatum = await GetUserDataForUser(user);
            if (user.UserDatum != null)
            {
                _context.UserData.Remove(user.UserDatum);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
