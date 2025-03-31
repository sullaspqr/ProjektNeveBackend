using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektNeveBackend.Models;
using System.Threading.Tasks;

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        private readonly BackendAlapContext _context;
        private readonly IEmailService _emailService;

        public RegistryController(BackendAlapContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Registry(User user)
        {
            try
            {
                if (await _context.Users.AnyAsync(f => f.FelhasznaloNev == user.FelhasznaloNev))
                {
                    return Conflict("Már létezik ilyen felhasználónév!");
                }

                if (await _context.Users.AnyAsync(f => f.Email == user.Email))
                {
                    return Conflict("Ezzel az e-mail címmel már regisztráltak!");
                }

                user.Jogosultsag = 1;
                user.Aktiv = 0;
                user.Hash = Program.CreateSHA256(user.Hash);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var emailSubject = "Regisztráció";
                var emailBody = $"https://localhost:7225/api/Registry?felhasznaloNev={user.FelhasznaloNev}&email={user.Email}";
                await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

                return Ok("Sikeres regisztráció. Fejezze be a regisztrációját az e-mail címére küldött link segítségével!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EndOfTheRegistry(string felhasznaloNev, string email)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(f => f.FelhasznaloNev == felhasznaloNev && f.Email == email);

                if (user == null)
                {
                    return NotFound("Sikertelen a regisztráció befejezése!");
                }

                user.Aktiv = 1;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Ok("A regisztráció befejezése sikeresen megtörtént.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}
