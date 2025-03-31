using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektNeveBackend.Models;

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly BackendAlapContext _context;

        public LogoutController(BackendAlapContext context)
        {
            _context = context;
        }

        [HttpPost("{uId}")]
        public async Task<IActionResult> Logout(string uId)
        {
            try
            {
                if (Program.LoggedInUsers.ContainsKey(uId))
                {
                    // Opcionális: naplózzuk a kijelentkezést az adatbázisba
                    var user = await _context.Users.FindAsync(Program.LoggedInUsers[uId].Id);
                    if (user != null)
                    {
                        user.LastLogout = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }

                    Program.LoggedInUsers.Remove(uId);
                    return Ok("Sikeres kijelentkezés.");
                }
                else
                {
                    return BadRequest("Sikertelen kijelentkezés: érvénytelen token.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
