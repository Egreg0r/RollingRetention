using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RollingRetention.Models;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RollingRetention.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivitysController : ControllerBase
    {
        private readonly UserActivityContext _context;
        public UserActivitysController(UserActivityContext context)
        {
            _context = context;

            if (_context.userActivitys.Count() == 0)
            {
                _context.userActivitys.Add(new UserActivity { UserId = 3000, LastActivityDate = DateTime.Now.Date, RegistrationDate = Convert.ToDateTime("11.11.2011") });
                _context.SaveChanges();
            }
        }

        //GET: api/<UserActivitysController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetUserActivitys()
        {
            return await _context.userActivitys.ToListAsync();
        }

        /// GET:api/<UserActivitysController>
        //[HttpGet(Name = "GetTodo")]
        //public ActionResult<IEnumerable<UserActivity>> GetAll()
        //{
        //    return _context.userActivitys.ToList();
        //}


        // GET api/<UserActivitysController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserActivity>> GetUserActivity(int id)
        {
            var activity = await _context.userActivitys.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        // POST api/<UserActivitysController>
        [HttpPost]
        public async Task<ActionResult<UserActivity>> Post(IEnumerable<UserActivity> activitys)
        {
            foreach (var a in activitys)
            {
                _context.userActivitys.Add(a);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", activitys);
        }

        // PUT api/<UserActivitysController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserActivity activity)
        {
            if (id != activity.Id)
            {
                return BadRequest();
            }

            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE api/<UserActivitysController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pizza = await _context.userActivitys.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }

            _context.userActivitys.Remove(pizza);
            await _context.SaveChangesAsync();

            return NoContent();


        }


        private bool UserActivityExists(int id)
        {
            return _context.userActivitys.Any(e => e.Id == id);
        }
    }
}
