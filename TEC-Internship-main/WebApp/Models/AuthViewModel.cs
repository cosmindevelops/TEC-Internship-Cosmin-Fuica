namespace WebApp.Models;

public class AuthViewModel
{
    public LoginModel Login { get; set; } = new LoginModel();
    public RegisterModel Register { get; set; } = new RegisterModel();
    public bool IsRegisterActive { get; set; } = false;
}