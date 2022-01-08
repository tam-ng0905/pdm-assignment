using System.ComponentModel.DataAnnotations.Schema;
using Application.Profiles;
using Domain;

namespace Application.Titles;

public class TitleDto
{
    public Guid Id { get; set; }
    
    public string Isbn { get; set; }
    public string Name { get; set; }
    
    [ForeignKey("Author")]
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    
    public int Stocks { get; set; }
    public int Pages { get; set; }
    public int PublishedYear { get; set; }
    public decimal Price { get; set; }
    
    public string OwnerName { get; set; }
    

    public ICollection<Profile> Profiles { get; set; }
}