using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;

namespace souschef.server.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager)
    {
        _userManager = userManager;
        _signinManager = signinManager;
    }

    /// <summary>
    /// Post endpoint to register for the application 
    /// </summary>
    /// <param name="RegisterDto">Register Dto with all the fields: ie username, email, password etc</param>
    /// <returns>Returns 200 ok or an error msg and status code</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        if (dto.Password != dto.PasswordConfirm)
            return new ContentResult() { Content = "Passwords do not match", StatusCode = 403 };

        var user = new ApplicationUser() { UserName = dto.UserName, Email = dto.Email };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new ContentResult() { Content = "Registration failed", StatusCode = 403 };
        }

        return Ok();
    }

    /// <summary>
    /// Post endpoint to login to the application  
    /// </summary>
    /// <param name="LoginDto">Register Dto with all the fields: ie email, and password</param>
    /// <returns>Returns 200 ok or an error msg and status code</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return new ContentResult() { Content = "Email or password invalid", StatusCode = 403 };
        }

        var result = await _signinManager.PasswordSignInAsync(user.UserName, dto.Password, false, true);

        if (!result.Succeeded)
        {
            return new ContentResult() { Content = "Login failed: Try again", StatusCode = 403 };
        }
        if (result.IsLockedOut)
        {
            return new ContentResult() { Content = "Account locked out", StatusCode = 403 };
        }

        //await _userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));


        //var jwt = JwtService.Generate(user.Id);


        // Response.Cookies.Append("jwt", jwt, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Expires = DateTime.Now.AddDays(30)
        // });

        var userDTO = new UserDTO
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            SkillLevel = user.SkillLevel
        };

        return Ok(userDTO);
    }


    /// <summary>
    /// Post endpoint to DeleteAccount from the application  
    /// </summary>
    /// <param name="LoginDto">Register Dto with all the fields: ie email, and password</param>
    /// <returns>Returns 200 ok or an error msg and status code</returns>
    [AllowAnonymous]
    [HttpPost("delete-account")]
    public async Task<IActionResult> DeleteAccount([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return new ContentResult() { Content = "User Not Found", StatusCode = 403 };
        }

        if (await _userManager.CheckPasswordAsync(user, dto.Password) == false)
        {
            return new ContentResult() { Content = "Invalid Password", StatusCode = 403 };
        }

        var result = await _signinManager.PasswordSignInAsync(user.UserName, dto.Password, false, true);

        if (!result.Succeeded)
        {
            return new ContentResult() { Content = "SignIn Failed: Try Again", StatusCode = 403 };
        }
        if (result.IsLockedOut)
        {
            return new ContentResult() { Content = "Account Locked Out", StatusCode = 403 };
        }

        await _userManager.DeleteAsync(user);

        return Ok("Account Deleted");
    }

    /// <summary>
    /// Post endpoint to EditUserAccount
    /// </summary>
    /// <param name="EditDTO"></param>
    /// <returns>Returns 200 ok or an error msg and status code</returns>
    [AllowAnonymous]
    [HttpPost("edit-user-account")]
    public async Task<IActionResult> EditUserAccount([FromBody] EditDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return new ContentResult() { Content = "User Not Found", StatusCode = 403 };
        }

        if (await _userManager.CheckPasswordAsync(user, dto.Password) == false)
        {
            return new ContentResult() { Content = "Invalid Password", StatusCode = 403 };
        }

        user.UserName = dto.NewName;
        user.Email = dto.NewEmail;

        await _userManager.UpdateAsync(user);

        return Ok("Account Updated");
    }


    /// <summary>
    /// Post endpoint to logout
    /// </summary>
    /// <returns>Returns 200 ok</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        Console.WriteLine("Log out");

        //Response.Cookies.Delete("jwt");

        await _signinManager.SignOutAsync();

        return Ok(new { message = "success" });
    }

    [HttpPost("set-user-skill")]
    public async Task<ActionResult> SetUserSkill(string userId, int skill)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.SkillLevel = skill;

        await _userManager.UpdateAsync(user);

        return Ok();
    }
}