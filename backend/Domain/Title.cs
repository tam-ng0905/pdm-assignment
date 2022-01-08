using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

//The schema for the book table
//Index by Name and Price

namespace Domain;
[Index(nameof(Name), nameof(Price))]
public class Title
{
    public Guid Id { get; set; }
    
    [MaxLength(10)]
    public string Isbn { get; set; }
    [MaxLength(150)]
    public string Name { get; set; }
    
    [ForeignKey("Author")]
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    public int Stocks { get; set; }
    public int Pages { get; set; }
    public int PublishedYear { get; set; }
    public decimal Price { get; set; }

    public ICollection<BookOwner> Owner { get; set; } = new List<BookOwner>();
}