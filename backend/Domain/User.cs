using Microsoft.AspNetCore.Identity;

//Set up the user schema

namespace Domain;
public class User : IdentityUser
{
    public string Name { set; get; }
    public ICollection<BookOwner> Titles { set; get; }
}