using CarUsed.InputModels;
using CarUsed.Models.InputModels;
using CarUsed.Services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarUsed.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManger _userManger;
        public UserController(IUserManger userManger)
        {
            _userManger = userManger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var result = await _userManger.Login(login.Email, login.Password);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RefreshToken()
        {
            var newToken = await _userManger.RefreshToken();
            return newToken != null ? Ok(newToken) : NotFound();
        }

        [HttpGet, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeToken()
        {
            var result = await _userManger.ChangeToken();
            return result != null ? Ok(result) : NotFound();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterModel registerModel)
        //{
        //    var result = await _userManger.Register(registerModel);
        //    return result != null ? Ok(result) : NotFound();
        //}

    }
}
