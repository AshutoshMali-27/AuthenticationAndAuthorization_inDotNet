using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotnetLearn.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppicationUser> usermanager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<AppicationUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            this.usermanager = userManager;
            this.roleManager = roleManager;
            _configuration=configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Registermodel model)
        {
            var userExist = await usermanager.FindByNameAsync(model.UserName);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new Response { Status="error",
                    Message
                = "User Already Exist"});

            }

            AppicationUser user = new AppicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
            };
            var result=await usermanager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "error",
                    Message
                 = "User Creation Failed"
                });
            }

            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Loginmodel model)
        {
            var user = await usermanager.FindByNameAsync(model.Username);
          if(user != null&& await usermanager.CheckPasswordAsync(user,model.Password))
            {
                var userRoles=await usermanager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secret"]));
                var token = new JwtSecurityToken(

                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWt:ValidAudience"],
                    expires: DateTime.Now.AddHours(5),
                    claims:authClaims,
                    signingCredentials:new SigningCredentials(authSigninKey,SecurityAlgorithms.HmacSha256)

                    );

                return Ok(new
                {
                    token=new JwtSecurityTokenHandler().WriteToken(token)
                }
                    );
            }
            return Unauthorized(); 

         }
    }
}
