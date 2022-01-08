using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


//The schema for author object
//Index by first name and last name

namespace Domain;

[Index(nameof(FirstName), nameof(LastName))]
public class Author
{
    public Guid Id { get; set; }
    
    [MaxLength(150)]
    public string  FirstName { get; set; }
    [MaxLength(150)]
    public string LastName { get; set; }
    
}