using Domain;
using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;

//Feed the database with seed data

namespace Persistence
{
    public class Seed
    {
        // Instantiate random number generator.  
        private static readonly Random _random = new Random();  
  
        // Generates a random number within a range.      
        public static int RandomNumber(int min, int max)  
        {  
            return _random.Next(min, max);  
        }  
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            
            //If there are already more than 1M rows in the book table, return 

            if (context.Titles.ToList().Count >= 1000000)
            {
                return;
                
            }
            
            //Set up the seed users
            var users = new List<User>
            {
                new User {Name = "Tam", UserName = "tam", Email = "tam@gmail.com"},
                new User {Name = "Hubert", UserName = "hubert", Email = "hubert@gmail.com"}
            };
            foreach (var user in users)
            {
                //Create each user with password
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }


            //Generate randomly 10000 authors
            var authors = new List<Author>();
            const int authorLimit = 10000;
            for (var i = 0; i < authorLimit; i++)
            {
                Console.WriteLine("creating author: " + i);
                authors.Add(
                    new Faker<Author>()
                        .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                        .RuleFor(o => o.LastName, f => f.Name.LastName())
                );
            }


            //User faker library 
            var faker = new Faker("en");
            
            //Generate randomly 1000000 titles based on 1000 authors from above
            var titles = new List<Title>();
            
            for (var i = 0; i < (authorLimit * 100); i++)
            {
                Console.WriteLine(users[0]);
                Console.WriteLine("creating book: " + i);
                Console.WriteLine(i % 2);
                
                //Set up book object with random data
                titles.Add(
                    new Title
                    {
                        Isbn = faker.Commerce.Ean8(),
                        Name = new Lorem(locale: "en").Sentence(5),
                        Author = authors[RandomNumber(0, authorLimit - 1)],
                        
                        //Generate up to 3000 pages for a book
                        Pages = faker.Random.Number(1, 3000),
                        
                        Stocks = faker.Random.Number(1, 50),
                        
                        //Generate a random year ranges from 1900 to 221
                        PublishedYear = (1900 + faker.Random.Number(0, 121)),
                        
                        Price = faker.Finance.Amount(),
                        Owner = new List<BookOwner>
                        {
                           new BookOwner
                           {
                               User = users[i % 2],
                               Owned = true
                           }
                        }
                    }
                );
            }
            
            await context.Titles.AddRangeAsync(titles);
            await context.SaveChangesAsync();
            

        }
    }
}