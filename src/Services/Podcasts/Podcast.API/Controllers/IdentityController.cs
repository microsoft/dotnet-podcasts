using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Podcast.API.Models;
using Podcast.Infrastructure.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Podcast.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private UserManager<ApplicationUser> _userManger;
    private IConfiguration _configuration;

    public IdentityController(UserManager<ApplicationUser> userManger, IConfiguration configuration)
    {
        _userManger = userManger;
        _configuration = configuration;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
    {

        if (ModelState.IsValid)
        {
            var identityUser = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManger.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {
                var addRole = await _userManger.AddToRoleAsync(identityUser, "User");
                return Ok("User created successfully !");
            }
            
            return BadRequest(result);
        }

        return BadRequest("Some properties are not valid");
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);

            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                return BadRequest("Invalid password !");

            var roles = await _userManger.GetRolesAsync(user);
            var claims = new List<Claim>();

            claims.Add(new Claim("Email", model.Email));
            claims.Add(new Claim("UserId", user.Id));
            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role));
            }



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["IdentitySettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["IdentitySettings:Issuer"],
                audience: _configuration["IdentitySettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Message = tokenAsString,ExpireDate=token.ValidTo});
        }

        return BadRequest("Some properties are not valid");
    }
}

