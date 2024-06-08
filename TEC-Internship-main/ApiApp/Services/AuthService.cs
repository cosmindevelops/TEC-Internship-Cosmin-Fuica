using ApiApp.Common.Dto;
using ApiApp.Common.Exceptions;
using ApiApp.Data;
using ApiApp.Services.Interfaces;
using Internship.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Services;

public class AuthService : IAuthService
{
    private readonly APIDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(APIDbContext context, IJwtTokenService jwtTokenService, IPasswordHasher<User> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    /// <summary>
    /// Registers a new user asynchronously.
    /// </summary>
    /// <param name="model">The registration model containing the user's information.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="model"/> is null.</exception>
    /// <exception cref="UserAlreadyExistsException">Thrown if the user already exists.</exception>
    /// <returns>A task representing the asynchronous registration operation.</returns>
    public async Task RegisterAsync(RegisterModelDto model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        if (await UserExistsAsync(model.Username, model.Email)) throw new UserAlreadyExistsException("User already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = model.Username,
            Email = model.Email,
            PasswordHash = _passwordHasher.HashPassword(null, model.Password)
        };

        await RegisterUserAsync(user, "User");
    }

    /// <summary>
    /// Logs in a user asynchronously.
    /// </summary>
    /// <param name="model">The login model containing the user's credentials.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="model"/> is null.</exception>
    /// <exception cref="InvalidCredentialsException">Thrown if the credentials are invalid.</exception>
    /// <returns>A task representing the asynchronous login operation. The task result contains the authentication response.</returns>
    public async Task<AuthResponseDto> LoginAsync(LoginModelDto model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var user = await FindByEmailAsync(model.Email);
        if (user == null) throw new InvalidCredentialsException("Invalid email or password.");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (verificationResult != PasswordVerificationResult.Success) throw new InvalidCredentialsException("Invalid email or password.");

        var roles = await GetUserRolesAsync(user);

        var token = _jwtTokenService.GenerateToken(user, roles);

        return new AuthResponseDto { UserId = user.Id, Token = token, Username = user.Username };
    }

    /// <summary>
    /// Checks if a user with the specified username or email already exists asynchronously.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <param name="email">The email to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the user exists.</returns>
    private async Task<bool> UserExistsAsync(string username, string email)
    {
        return await _context.Users.AnyAsync(u => u.Username == username || u.Email == email);
    }

    /// <summary>
    /// Registers a new user with a specified role asynchronously.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <param name="roleName">The role name to assign to the user.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="user"/> is null.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains the registered user.</returns>
    private async Task<User> RegisterUserAsync(User user, string roleName)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role != null)
        {
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        return user;
    }

    /// <summary>
    /// Finds a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    private async Task<User> FindByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    /// <summary>
    /// Finds a user by email asynchronously.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    private async Task<User> FindByEmailAsync(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Gets the roles of a specified user asynchronously.
    /// </summary>
    /// <param name="user">The user to get roles for.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="user"/> is null.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of role names.</returns>
    private async Task<List<string>> GetUserRolesAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        return await _context.UserRoles.Where(ur => ur.UserId == user.Id)
                                        .Include(ur => ur.Role)
                                        .Select(ur => ur.Role.Name)
                                        .ToListAsync();
    }

    /// <summary>
    /// Finds a user by ID asynchronously.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    private async Task<User> FindByIdAsync(Guid userId)
    {
        return await _context.Users
                             .Include(u => u.UserRoles)
                             .ThenInclude(ur => ur.Role)
                             .SingleOrDefaultAsync(u => u.Id == userId);
    }
}