namespace API.DTOs;


//DATA TRANSFER OBJECT
//CARRYING DATA BETWEEN PROCESSES
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}