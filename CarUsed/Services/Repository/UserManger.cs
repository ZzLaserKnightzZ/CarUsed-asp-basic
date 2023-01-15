using CarUsed.InputModels;
using CarUsed.Models;
using CarUsed.Models.OutPutModels;
using CarUsed.Services.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarUsed.Services.Repository
{
    public class UserManger : IUserManger
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly RolesManager _roleManager;
        public UserManger(IConfiguration configuration, AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _appDbContext = dbContext;
            _contextAccessor = httpContextAccessor;
            _roleManager = new RolesManager(dbContext);
        }
        public async Task<TokenModel?> ChangeToken()
        {
            try
            {
                var userId = _contextAccessor?.HttpContext?.User?.FindFirstValue("UserId");
                var userName = _contextAccessor?.HttpContext?.User?.FindFirstValue("UserName");
                var userEmail = _contextAccessor?.HttpContext?.User?.FindFirstValue("Email");
                var token = new TokenModel
                {
                    Token = JWTClaimsRoles(DateTime.Now.AddMinutes(5), userId.ToString(), userName, userEmail, new string[] { "ADMIN" })
                };
                return await Task.FromResult(token);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<LoginModel?> Login(string username, string password)
        {
            try
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password); //no encrypt
                var roles = new List<string>(){ "ADMIN"}; //no table for beta
                if (user != null)
                {
                    //var role = await _roleManager.UserGetAllRole(user.UserId);
                    var returnLogin = new LoginModel
                    {
                        UserName = user.Name,
                        Email = user.Email,
                        RefreshToken = JWTClaimsRoles(DateTime.Now.AddDays(60), user.UserId.ToString(), user.Name, user.Email, new string[] { "ADMIN" }),
                        Token = JWTClaimsRoles(DateTime.Now.AddMinutes(5), user.UserId.ToString(), user.Name, user.Email, new string[] { "ADMIN" }),
                        Roles = roles
                    };
                    return returnLogin;
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<RefreshTokenModel?> RefreshToken()
        {
            try
            {
                var userId = _contextAccessor?.HttpContext?.User?.FindFirstValue("UserId");
                var user = await _appDbContext.Users.FirstOrDefaultAsync(_ => _.UserId.Equals(Guid.Parse(userId)));

                var refToken = new RefreshTokenModel
                {
                    RefreshToken = JWTClaimsRoles(DateTime.Now.AddDays(60), user.UserId.ToString(), user.Name, user.Email, new string[] { "ADMIN" }),
                    Token = JWTClaimsRoles(DateTime.Now.AddMinutes(5), user.UserId.ToString(), user.Name, user.Email, new string[] { "ADMIN" }),
                };

                return refToken;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<LoginModel?> Register(RegisterModel user)
        {
            try
            {
                var containUser = await _appDbContext.Users.AnyAsync(x => x.Email.Equals(user.Email));
                if (!containUser)
                {
                    await _appDbContext.Users.AddAsync(new User { Email = user.Email, Name = user.Email, Password = user.Password });
                    await _appDbContext.SaveChangesAsync();
                    var newUser = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(user.Email));
                    return new LoginModel
                    {
                        Email = user.Email,
                        UserName = user.Email,
                        Token = JWTClaimsRoles(DateTime.Now.AddMinutes(1), newUser.UserId.ToString(), newUser.Name, newUser.Email, new string[] { "ADMIN" }),
                        RefreshToken = JWTClaimsRoles(DateTime.Now.AddMinutes(5), newUser.UserId.ToString(), newUser.Name, newUser.Email, new string[] { "ADMIN" })
                    };
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private string? JWTClaimsRoles(DateTime exp, string userId, string userName, string email, string[] claimRole)
        {
            try
            {

                var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", userId),
                        new Claim("UserName", userName),
                        new Claim("Email", email),
                    };

                foreach (string c in claimRole)
                {
                    claims.Add(new Claim(ClaimTypes.Role, c));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: exp,
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

            }
            return null;

        }

    }
}
