using CarUsed.InputModels;
using CarUsed.Models.OutPutModels;
using CarUsed.Models;

namespace CarUsed.Services.IRepository
{
    public interface IUserManger
    {
        Task<LoginModel?> Login(string username, string password);
        Task<LoginModel?> Register(RegisterModel user);
        Task<RefreshTokenModel?> RefreshToken ();
        Task<TokenModel?> ChangeToken();
    }
}
