namespace API.DTOs;


//DATA TRANSFER OBJECT
//CARRYING DATA BETWEEN PROCESSES
public class RegisterDto
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}