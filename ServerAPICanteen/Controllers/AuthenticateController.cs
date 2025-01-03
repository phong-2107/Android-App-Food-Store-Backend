    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using ServerAPICanteen.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace ServerAPICanteen.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthenticateController : ControllerBase
        {
            private readonly UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _configuration;

            public AuthenticateController(
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _configuration = configuration;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegistrationModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Kiểm tra xem người dùng đã tồn tại chưa
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return StatusCode(StatusCodes.Status400BadRequest, new { Status = false, Message = "User already exists" });

                // Tạo đối tượng User với PhoneNumber
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber, 
                    Initials = model.Initials,
                    Active = true,
                    CreDate = DateTime.UtcNow
                };

                // Tạo người dùng
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = false, Message = "User creation failed" });

                // Gán Role nếu được cung cấp
                if (!string.IsNullOrEmpty(model.Role))
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                return Ok(new { Status = true, Message = "User created successfully" });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized(new { Status = false, Message = "Invalid username or password" });

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GenerateToken(authClaims);

                // Bao gồm thông tin chi tiết của người dùng trong phản hồi
                return Ok(new
                {
                    Status = true,
                    Message = "Logged in successfully", 
                    Token = token,
                    UserDetails = new
                    {
                        user.UserName,
                        user.Email,
                        user.PhoneNumber,
                        user.Fullname,
                        Roles = userRoles
                    }
                });
            }


            //Thêm chức năng đăng xuất
            //Chỉ cần thông báo đăng xuất thành công ở server, không cần ghi thêm dữ liệu
            [HttpPost("logout")]
            [Authorize]
            public IActionResult Logout()
            {
                // Không cần làm gì ở phía server vì JWT là stateless
                return Ok(new { Status = true, Message = "Logged out successfully" });
            }


            private string GenerateToken(IEnumerable<Claim> claims)
            {
                var jwtSettings = _configuration.GetSection("JWTKey");
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["TokenExpiryTimeInHour"])),
                    Issuer = jwtSettings["ValidIssuer"],
                    Audience = jwtSettings["ValidAudience"],
                    SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

        }


    }
