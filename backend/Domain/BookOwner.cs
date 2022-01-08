
//The schema for the linking table for users and books

namespace Domain;
public class BookOwner
{
    public string UserId { get; set; }
    public User User { get; set; }
    public Guid BookId { get; set; }
    public Title Title { get; set; }
    public bool Owned { get; set; }
    
}