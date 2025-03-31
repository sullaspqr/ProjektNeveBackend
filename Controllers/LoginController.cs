using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektNeveBackend.DTOs;
using ProjektNeveBackend.Models;

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly BackendAlapContext _context;
        
        public LoginController(BackendAlapContext context)
        {
            _context = context;
        }

        [HttpPost("GetSalt/{felhasznaloNev}")]
        public async Task<IActionResult> GetSalt(string felhasznaloNev)
        {
            try
            {
                User response = await _context.Users
                    .FirstOrDefaultAsync(f => f.FelhasznaloNev == felhasznaloNev);
                    
                return response == null ? BadRequest("Hiba") : Ok(response.Salt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                string Hash = Program.CreateSHA256(loginDTO.TmpHash);
                User loggedUser = await _context.Users
                    .FirstOrDefaultAsync(f => f.FelhasznaloNev == loginDTO.LoginName && f.Hash == Hash);
                    
                if (loggedUser != null && loggedUser.Aktiv == 1)
                {
                    string token = Guid.NewGuid().ToString();
                    lock (Program.LoggedInUsers)
                    {
                        Program.LoggedInUsers.Add(token, loggedUser);
                    }
                    return Ok(new LoggedUser { 
                        Name = loggedUser.TeljesNev, 
                        Email = loggedUser.Email, 
                        Permission = loggedUser.Jogosultsag, 
                        ProfilePicturePath = loggedUser.ProfilKepUtvonal, 
                        Token = token 
                    });
                }
                else
                {
                    return BadRequest("Hibás név vagy jelszó/inaktív felhasználó!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new LoggedUser { 
                    Permission = -1, 
                    Name = ex.Message, 
                    ProfilePicturePath = "", 
                    Email = "" 
                });
            }
        }
    }
}
