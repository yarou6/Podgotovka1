using API.DB;
using API.Models.DTO.Auth;
using API.Services.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ExamApiContext db;
    private readonly IPasswordHasherService passwordHasher;
    private readonly IPasswordValidationService passwordValidation;
    public AuthController(ExamApiContext db,  IPasswordHasherService passwordHasher, IPasswordValidationService passwordValidation)
    {
        this.db = db;
        this.passwordHasher = passwordHasher;
        this.passwordValidation = passwordValidation;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto request)
    {
        if(request.Password != request.ConfirmPassword)
            return BadRequest("пароли не совподают");
        
        var passwordError = passwordValidation.ValidatePasswordPolicy(request.Password);
        if(passwordError is not null)
            return BadRequest(passwordError);

        var role = await db.Roles.FirstOrDefaultAsync(r => r.Title == "client");
        if(role is null)
            return StatusCode(500, "Роль 'client' не найдена");

        if (await db.Users.AnyAsync(u => u.Email == request.Email))
            return Conflict("Почта уже зарегистрированна");
        
        var now = DateTime.UtcNow;

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            RoleId = role.Id,
            CreatedAt = now,
        };
        
        db.Users.Add(user);
        await db.SaveChangesAsync();
        
        return Ok(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = role.Title,
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto request)
    {
        var user = await db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Неверный логин/почта или пароль");

        return Ok(new AuthResponseDto
        {    
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.Title
        });
    }
    
}
