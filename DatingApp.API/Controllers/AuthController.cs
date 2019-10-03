using System.Threading.Tasks;
using DatingApp.API.Data.Repositories.Interfaces;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userInfoDto) 
        {
            userInfoDto.Username = userInfoDto.Username.ToLower();

            if(await _authRepo.UserExists(userInfoDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User 
            {
                UserName = userInfoDto.Username
            };

            var registerUser = await _authRepo.Register(userToCreate, userInfoDto.Password);

            return StatusCode(201);
        }
    }
}