using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WeatherOfCity.Input;
using WeatherOfCity.Models;
using WeatherOfCity.Sevices;
namespace WeatherOfCity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WeatherContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserSevices _userSevices;
        public AuthController(WeatherContext weatherContext, IConfiguration configuration, IUserSevices userSevices)
        {
            _context = weatherContext;
            _configuration = configuration;
            _userSevices = userSevices;
        }
        [HttpGet,Authorize("admin")]
        public async Task<IActionResult> GetUser()
        {
             var result = _context.Users.ToList();
           return Ok(result);
        }
        [HttpPost("RegisteUser")]
        #region Create User
        public async Task<IActionResult> Create_User(UserInput input)
        {
            var user = new Users();
            CreatePassHash(input.Password, out byte[] passHash, out byte[] passSalt);
            user.UserName = input.UserName;
            user.Password = input.Password;
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;
            user.Email = input.Email;
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        private void CreatePassHash(string password, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion
        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> User_Login(LoginInput input)
        {
            var check = _context.Users.SingleOrDefault(opt => opt.UserName == input.UserName && opt.Password == input.Password) ;
            if (check == null) return BadRequest("UserName không tồn tại");
            if (!VerifiPassword(input.Password, check.PasswordSalt, check.PasswordHash)) return BadRequest("Sai mật khẩu rồi, cút");
            var result = CreateToken(check);
            return Ok(result);
        }
        [HttpPost("updateRole"),Authorize("admin")]
        public async Task<IActionResult> Update_Role(UpdateRoleUserInput input)
        {
            try
            {
                _userSevices.Insert_UpdateUser(input);
                return Ok();
            }
            catch(Exception ex)
            {
                return null ;
            }
        }
        private bool VerifiPassword(string p, byte[] salt, byte[] hash)
        {
            using (var temp = new HMACSHA512(salt))
            {
                var check = temp.ComputeHash(System.Text.Encoding.UTF8.GetBytes(p));
                return check.SequenceEqual(hash);
            }
        }
        private string CreateToken(Users user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.role==true?"admin":"user"),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(10),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        #endregion
    }
}
