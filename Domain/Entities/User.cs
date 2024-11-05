using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
	public string Role { get; set; } 
	public string RefreshToken { get; set; } 
	public DateTime RefreshTokenExpiry { get; set; }
}
