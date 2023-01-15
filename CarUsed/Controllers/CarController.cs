using CarUsed.Models.InputModels;
using CarUsed.Services.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarUsed.Controllers
{
    [Route("/[controller]/[action]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarManager _carManager;
        public CarController(ICarManager carManager)
        {
            _carManager = carManager;
        }

        [HttpPost,Authorize(Roles ="ADMIN")]
        public async Task<IActionResult> AddCar([FromForm] AddCarModel addCarModel)
        {
            var result = await _carManager.AddCar(addCarModel);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            var result = await _carManager.Delete(carId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCar()
        {
            return Ok(await _carManager.GetAllCar());
        }
        [HttpGet, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UserOnSell()
        {
            return Ok(await _carManager.UserOnSell());
        }

    }
}
