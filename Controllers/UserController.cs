using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektNeveBackend.Models;

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BackendAlapContext _context;

        public UserController(BackendAlapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(string uId)
        {
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag == 9)
            {
                try
                {
                    return Ok(_context.Users.Include(f => f.JogosultsagNavigation).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }

        [HttpGet("{uId},{id}")]
        public async Task<IActionResult> Get(string uId, int id)
        {
            uId = "a95c9c2a-18e6-4eac-b547-3b217958c84a";
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag == 9)
            {
                try
                {
                    return Ok(await _context.Users.Include(f => f.JogosultsagNavigation)
                        .FirstOrDefaultAsync(f => f.Id == id));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }

        [HttpGet("uId,felhasznaloNev")]
        public IActionResult GetNev(string uId, string felhasznaloNev)
        {
            uId = "a95c9c2a-18e6-4eac-b547-3b217958c84a";
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag > 6)
            {
                try
                {
                    return Ok(_context.Users.FirstOrDefault(f => f.FelhasznaloNev == felhasznaloNev));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }

        [HttpPost("{uId}")]
        public async Task<IActionResult> Post(string uId, User user)
        {
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag == 9)
            {
                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok("Sikeres felhasználó hozzáadás");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }

        [HttpPut("{uId}")]
        public async Task<IActionResult> Put(string uId, User user)
        {
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag == 9)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok("Sikeres felhasználó módosítás");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }

        [HttpDelete("{uId},{id}")]
        public async Task<IActionResult> Delete(string uId, int id)
        {
            if (Program.LoggedInUsers.ContainsKey(uId) && Program.LoggedInUsers[uId].Jogosultsag == 9)
            {
                try
                {
                    User user = new User { Id = id };
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok("Sikeres felhasználó törlés.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Nincs joga a művelethez!");
            }
        }
    }
}
