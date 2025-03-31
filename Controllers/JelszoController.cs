using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektNeveBackend.Models;
using ProjektNeveBackend.Services; // Assuming you'll move helper methods here

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JelszoController : ControllerBase
    {
        private readonly BackendAlapContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;

        public JelszoController(
            BackendAlapContext context,
            IPasswordService passwordService,
            IEmailService emailService)
        {
            _context = context;
            _passwordService = passwordService;
            _emailService = emailService;
        }

        [HttpPost("{loginName},{oldPassword},{newPassword}")]
        public async Task<IActionResult> JelszoMosositas(string loginName, string oldPassword, string newPassword)
        {
            try
            {
                User? user = _context.Users.FirstOrDefault(f => f.FelhasznaloNev == loginName);
                if (user != null)
                {
                    if (_passwordService.VerifyPassword(oldPassword, user.Hash))
                    {
                        user.Hash = _passwordService.HashPassword(newPassword);
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                        return Ok("A jelszó módosítása sikeresen megtörtént.");
                    }
                    else
                    {
                        return StatusCode(201, "Hibás a régi jelszó!");
                    }
                }
                else
                {
                    return BadRequest("Nincs ilyen nevű felhasználó!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{Email}")]
        public async Task<IActionResult> ElfelejtettJelszo(string Email)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(f => f.Email == Email);
                if (user != null)
                {
                    string jelszo = _passwordService.GenerateRandomPassword(16);
                    user.Hash = _passwordService.HashPasswordWithSalt(jelszo, user.Salt);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    
                    await _emailService.SendEmailAsync(user.Email, "Elfelejtett jelszó", $"Az új jelszava: {jelszo}");
                    return Ok("E-mail küldése megtörtént.");
                }
                else
                {
                    return StatusCode(210, "Nincs ilyen e-Mail cím!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(211, ex.Message);
            }
        }
    }
}
