using ApiApp.Common.Dto;
using ApiApp.Common.Exceptions;
using ApiApp.Data;
using ApiApp.Model;
using ApiApp.Services;
using ApiApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Entity;
using System.Linq.Expressions;

public class AuthServiceTests
{
    private readonly Mock<APIDbContext> _contextMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _contextMock = new Mock<APIDbContext>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _authService = new AuthService(_contextMock.Object, _jwtTokenServiceMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_NullModel_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.RegisterAsync(null));
    }

    [Fact]
    public async Task RegisterAsync_UserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        // Arrange
        var model = new RegisterModelDto { Username = "testuser", Email = "test@example.com", Password = "password" };
        _contextMock.Setup(c => Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(c.Users, It.IsAny<Expression<Func<User, bool>>>(), default))
                    .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _authService.RegisterAsync(model));
    }

    [Fact]
    public async Task RegisterAsync_ValidModel_RegistersUserSuccessfully()
    {
        // Arrange
        var model = new RegisterModelDto { Username = "testuser", Email = "test@example.com", Password = "password" };
        _contextMock.Setup(c => Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.AnyAsync(c.Users, It.IsAny<Expression<Func<User, bool>>>(), default))
                    .ReturnsAsync(false);
        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<User>(), model.Password))
                           .Returns("hashedpassword");

        // Act
        await _authService.RegisterAsync(model);

        // Assert
        _contextMock.Verify(c => c.Users.AddAsync(It.IsAny<User>(), default), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Exactly(2));
    }

    [Fact]
    public async Task LoginAsync_NullModel_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.LoginAsync(null));
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var model = new LoginModelDto { Email = "test@example.com", Password = "password" };
        _contextMock.Setup(c => Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(c.Users, It.IsAny<Expression<Func<User, bool>>>(), default))
                    .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.LoginAsync(model));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var model = new LoginModelDto { Email = "test@example.com", Password = "wrongpassword" };
        var user = new User { Email = model.Email, PasswordHash = "hashedpassword" };
        _contextMock.Setup(c => Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(c.Users, It.IsAny<Expression<Func<User, bool>>>(), default))
                    .ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, model.Password))
                           .Returns(PasswordVerificationResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.LoginAsync(model));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponseDto()
    {
        // Arrange
        var model = new LoginModelDto { Email = "test@example.com", Password = "password" };
        var user = new User { Id = Guid.NewGuid(), Email = model.Email, PasswordHash = "hashedpassword" };
        _contextMock.Setup(c => Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(c.Users, It.IsAny<Expression<Func<User, bool>>>(), default))
                    .ReturnsAsync(user);
        _passwordHasherMock.Setup(p => p.VerifyHashedPassword(user, user.PasswordHash, model.Password))
                           .Returns(PasswordVerificationResult.Success);
        _jwtTokenServiceMock.Setup(j => j.GenerateToken(user, It.IsAny<List<string>>()))
                            .Returns("generatedToken");

        // Act
        var result = await _authService.LoginAsync(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal("generatedToken", result.Token);
    }
}