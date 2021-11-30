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

            //if (_context.userActivitys.Count() == 0)
            //{
            //    _context.userActivitys.Add(new UserActivity { UserId = 3000, LastActivityDate = DateTime.Now.Date, RegistrationDate = Convert.ToDateTime("11.11.2020") });
            //    _context.userActivitys.Add(new UserActivity { UserId = 2000, LastActivityDate = Convert.ToDateTime("23.11.2021"), RegistrationDate = Convert.ToDateTime("01.01.2021") });
            //    _context.SaveChanges();
            //}
        }
        private static List<UserActivity> tableUser = new List<UserActivity>();

        //GET: api/<UserActivitysController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserActivity>>> GetLast()
        {
            var five = await _context.userActivitys.ToListAsync();
            return five.TakeLast(5).ToList();
        }


        // GET api/<UserActivitysController>/5
        [HttpGet("{day}")]
        public async Task<ActionResult<decimal>> GetUserActivity(int day)
        {
            //стартовая дата для подсчета
            var startDate = DateTime.Today.AddDays(-day);

            // выбираем все позиции где LastActivity не меньше нужной даты. 
            var activityList = await _context.userActivitys.Where(user => user.LastActivityDate >= startDate).Select(user =>user.Id).ToListAsync();
            var registrationList = await _context.userActivitys.Where(user => user.RegistrationDate <= startDate).Select(user => user.Id).ToListAsync();

            decimal metric = (decimal) activityList.Count / registrationList.Count * 100;

            return Decimal.Round(metric);
        }

        [HttpGet("Gistograma")]
        public async Task<ActionResult<List<int>>> GetGistogramma()
        {
            var users = await _context.userActivitys.Select(user => new {user.RegistrationDate, user.LastActivityDate}).ToListAsync();

            //Лист для хранения кол-ва дней между LastActivity и Registration (длительность жизни)
            List<int> rolling = new List<int>();

            foreach (var item in users)
            {
                var days = item.LastActivityDate.Subtract(item.RegistrationDate).Days;
                rolling.Add(days);
            }
            return rolling;
        }

        //Save table in base
        // POST api/<UserActivitysController>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserActivity>>> Save(IEnumerable<UserActivity> activitys)
        {
            foreach (var item in activitys)
            {
                _context.userActivitys.Add(new UserActivity { UserId = item.UserId, LastActivityDate = item.LastActivityDate, RegistrationDate=item.RegistrationDate }) ;
            }
            await _context.SaveChangesAsync();
            return Ok();
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
