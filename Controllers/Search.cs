using MembershipSystem.Data;
using MembershipSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MembershipSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Searches for users based on provided criteria such as username, email, or ID.
        /// </summary>
        /// <param name="Username">The username to search for (optional).</param>
        /// <param name="Email">The email to search for (optional).</param>
        /// <param name="Id">The ID to search for (optional).</param>
        /// <response code="200">Returns the list of users found</response>
        /// <response code="404">No users found matching the criteria</response>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(
            [FromQuery] string? Username,
            [FromQuery] string? Email,
            [FromQuery] int? Id)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(Username))
            {
                query = query.Where(u => u.Username.Contains(Username));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                query = query.Where(u => u.Email.Contains(Email));
            }

            if (Id.HasValue)
            {
                query = query.Where(u => u.Id == Id.Value);
            }

            var Users = await query.ToListAsync();

            if (Users.Count == 0)
            {
                return NotFound("No users found matching the criteria.");
            }

            return Ok(Users);
        }
    }
}


