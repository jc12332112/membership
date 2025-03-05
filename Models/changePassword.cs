using System.ComponentModel.DataAnnotations;

public class changePassword
{
    public string Username { get; set; } 
    public string Email { get; set; }    

    [Required]
    public string NewPassword { get; set; } 
}
