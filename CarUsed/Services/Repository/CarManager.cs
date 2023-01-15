using CarUsed.Models;
using CarUsed.Models.InputModels;
using CarUsed.Services.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarUsed.Services.Repository
{

    public class CarManager : ICarManager
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly ILogger<CarManager> _logger;
        public CarManager(IWebHostEnvironment hostEnvironment, AppDbContext context, IHttpContextAccessor httpContext, ILogger<CarManager> loger)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _httpContextAccessor = httpContext;
            _logger = loger;
        }

        public async Task<Car?> AddCar(AddCarModel car)
        {
            var folderName = Guid.NewGuid().ToString().Substring(0, 5);
            var savePath = Path.Combine("CarImages", folderName);
            try
            {
                var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue("userId");
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId.Equals(Guid.Parse(userId)));
                var createPath = Path.Combine(_hostEnvironment.WebRootPath, savePath);
                if (!Directory.Exists(createPath))
                {
                    Directory.CreateDirectory(createPath);
                }

                List<CarImages> carImages = new List<CarImages>();
                //save sub image
                car.Images.ForEach(async x =>
                {
                    var pathFile = Path.Combine(savePath, x.FileName);
                    carImages.Add(new CarImages { Path = pathFile });

                    var path = Path.Combine(_hostEnvironment.WebRootPath, pathFile);
                    if (!File.Exists(path) && x.FileName.ToLower().EndsWith(".png") || x.FileName.ToLower().EndsWith(".jpg"))
                        await x.CopyToAsync(new FileStream(path, FileMode.Create));
                });

                //save show image 
                var saveShowImagePath = Path.Combine(savePath, car.ShowImagePath.FileName);
                var saveShowImg = Path.Combine(_hostEnvironment.WebRootPath, saveShowImagePath);
                if (!File.Exists(saveShowImg) && car.ShowImagePath.FileName.ToLower().EndsWith(".png") || car.ShowImagePath.FileName.ToLower().EndsWith(".jpg"))
                    await car.ShowImagePath.CopyToAsync(new FileStream(saveShowImg, FileMode.Create));

                var newCar = new Car
                {
                    Bran = car.Bran,
                    Color = car.Color,
                    Detail = car.Detail,
                    Down = car.Down,
                    Energy = car.Energy,
                    Gear = car.Gear,
                    Series = car.Series,
                    Type = car.Type,
                    Price = car.Price,
                    Year = car.Year,
                    ShowImagePath = saveShowImagePath,
                    Images = carImages,
                    User = user
                };

                await _context.Cars.AddAsync(newCar);
                await _context.SaveChangesAsync();
                return newCar;
            }
            catch (Exception ex) //save fail
            {
                var createFailPath = Path.Combine(_hostEnvironment.WebRootPath, savePath);
                Directory.Delete(createFailPath, true);
                _logger.LogWarning(ex.Message);
                return null;
            }


        }



        public async Task<bool> Delete(int carId)
        {
            try
            {

                var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue("UserId");
                var deleteCar = await _context.Cars.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == carId && x.UserId.Equals(Guid.Parse(userId))); 
                if (deleteCar != null)
                {
                    string delDirName = new FileInfo(Path.Combine(_hostEnvironment.WebRootPath, deleteCar.ShowImagePath)).DirectoryName;
                    Directory.Delete(delDirName, true);
                    _context.CarImages.RemoveRange(deleteCar.Images);
                    _context.Cars.Remove(deleteCar);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }
            return false;
        }

        public async Task<List<Car>> GetAllCar()
        {
            return await _context.Cars.Include(x => x.Images).ToListAsync();
        }

        public async Task<IList<Car>> UserOnSell()
        {
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue("UserId");
            var p = _context.Users.Include(x => x.Cars).Where(x => x.UserId.Equals(Guid.Parse(userId))).Select(x => x.Cars).FirstOrDefault();
            return p;
        }
    }
}
