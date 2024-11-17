using LibraryAPI.Models;

namespace LibraryAPI.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(LibraryDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (dbContext.Books.Any() || dbContext.Authors.Any() || dbContext.Publishers.Any())
        {
            return;
        }
        
        var authors = new List<Author>
        {
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1975, 5, 20)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1980, 11, 15)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Emily",
                LastName = "Bronte",
                DateOfBirth = new DateTime(1818, 7, 30)
            }
        };
        dbContext.Authors.AddRange(authors);
        
        var publishers = new List<Publisher>
        {
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Penguin Books",
                Address = "375 Hudson St, New York, NY",
                FoundedYear = 1935
            },
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "HarperCollins",
                Address = "195 Broadway, New York, NY",
                FoundedYear = 1989
            }
        };
        dbContext.Publishers.AddRange(publishers);
        
        var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Mystery of the Haunted House",
                ISBN = "978-1-2345-6789-0",
                Category = BookCategory.Mystery,
                AuthorId = authors[0].Id,
                PublisherId = publishers[0].Id,
                ReleaseDate = new DateTime(2005, 6, 15)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Science of Nature",
                ISBN = "978-0-1234-5678-9",
                Category = BookCategory.Science,
                AuthorId = authors[1].Id,
                PublisherId = publishers[1].Id,
                ReleaseDate = new DateTime(2012, 9, 22)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Life and Biography",
                ISBN = "978-9-8765-4321-0",
                Category = BookCategory.Biography,
                AuthorId = authors[2].Id,
                PublisherId = publishers[0].Id,
                ReleaseDate = new DateTime(2008, 3, 10)
            }
        };
        dbContext.Books.AddRange(books);
        
        await dbContext.SaveChangesAsync();
    }
}