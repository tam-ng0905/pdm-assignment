using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//Set up the context for the database

namespace Persistence;
public class DataContext: IdentityDbContext<User>
{
        public DataContext(DbContextOptions options) : base(options)
        {
                
        }
        
        public DbSet<BookOwner> Owners { get; set; }
        
        public DbSet<Author> Authors { get; set; }
        
        public DbSet<Title> Titles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
                base.OnModelCreating(builder);

                //Forming a new key for the table
                builder.Entity<BookOwner>(x => x.HasKey(a => new {a.UserId, a.BookId}));
                builder.Entity<BookOwner>()
                        .HasOne(u => u.User)
                        .WithMany(b => b.Titles)
                        .HasForeignKey(aa => aa.UserId);

                builder.Entity<BookOwner>()
                        .HasOne(u => u.Title)
                        .WithMany(a => a.Owner)
                        .HasForeignKey(f => f.BookId);


        }

        
        
}