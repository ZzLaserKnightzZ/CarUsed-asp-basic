using CarUsed.Models;
using CarUsed.Models.InputModels;

namespace CarUsed.Services.IRepository
{
    public interface ICarManager
    {
        Task<Car> AddCar(AddCarModel car);
        Task<bool> Delete(int carId);
        Task<IList<Car>> UserOnSell(); //skip take
        Task<List<Car>> GetAllCar(); //skip take
    }
}
