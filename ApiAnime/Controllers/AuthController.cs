using ApiAnime.Models;
using ApiAnime.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if(!username.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                var token = TokenService.GenerateToke(new Anime());
                return Ok(token);
            }

            return BadRequest("Usuário ou senha inválido(s)");
        }
    }
}
