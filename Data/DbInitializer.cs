using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryAPI.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(LibraryDbContext dbContext)
    {
        // await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        
        if (!dbContext.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { Name = "Admin" },
                new Role { Name = "Manager" },
                new Role { Name = "User" }
            };

            dbContext.Roles.AddRange(roles);
            await dbContext.SaveChangesAsync();
        }
        
        if (!dbContext.Users.Any())
        {
            var user = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                UserRoles = new List<UserRole>()
            };
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "admin123");
            
            var adminRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    Role = adminRole
                });
            }

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        
        if (dbContext.Books.Any() || dbContext.Authors.Any() || dbContext.Publishers.Any())
        {
            return;
        }
        
        var authors = new List<Author>
        {
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "J.K.",
                LastName = "Rowling",
                DateOfBirth = new DateTime(1965, 7, 31)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "George",
                LastName = "Orwell",
                DateOfBirth = new DateTime(1903, 6, 25)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Mark",
                LastName = "Twain",
                DateOfBirth = new DateTime(1835, 11, 30)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "Agatha",
                LastName = "Christie",
                DateOfBirth = new DateTime(1890, 9, 15)
            },
            new Author
            {
                Id = Guid.NewGuid(),
                FirstName = "J.R.R.",
                LastName = "Tolkien",
                DateOfBirth = new DateTime(1892, 1, 3)
            }
        };
        dbContext.Authors.AddRange(authors);
        
        var publishers = new List<Publisher>
        {
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Penguin Random House",
                Address = "1745 Broadway, New York, NY",
                FoundedYear = 1927
            },
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "HarperCollins",
                Address = "195 Broadway, New York, NY",
                FoundedYear = 1989
            },
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Simon & Schuster",
                Address = "1230 Avenue of the Americas, New York, NY",
                FoundedYear = 1924
            },
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Macmillan",
                Address = "120 Broadway, New York, NY",
                FoundedYear = 1843
            },
            new Publisher
            {
                Id = Guid.NewGuid(),
                Name = "Hachette Book Group",
                Address = "1290 Avenue of the Americas, New York, NY",
                FoundedYear = 2006
            }
        };
        dbContext.Publishers.AddRange(publishers);

        var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Philosopher's Stone",
                ISBN = "978-0-7475-3269-9",
                Category = BookCategory.Fantasy,
                AuthorId = authors[0].Id,
                PublisherId = publishers[0].Id,
                ReleaseDate = new DateTime(1997, 6, 26)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "1984",
                ISBN = "978-0-452-28423-4",
                Category = BookCategory.NonFiction,
                AuthorId = authors[1].Id,
                PublisherId = publishers[1].Id,
                ReleaseDate = new DateTime(1949, 6, 8)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Adventures of Huckleberry Finn",
                ISBN = "978-0-141-43915-3",
                Category = BookCategory.Fantasy,
                AuthorId = authors[2].Id,
                PublisherId = publishers[2].Id,
                ReleaseDate = new DateTime(1884, 12, 10)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Murder on the Orient Express",
                ISBN = "978-0-006-29868-6",
                Category = BookCategory.Mystery,
                AuthorId = authors[3].Id,
                PublisherId = publishers[3].Id,
                ReleaseDate = new DateTime(1934, 1, 1)
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Hobbit",
                ISBN = "978-0-618-00221-3",
                Category = BookCategory.Fantasy,
                AuthorId = authors[4].Id,
                PublisherId = publishers[4].Id,
                ReleaseDate = new DateTime(1937, 9, 21)
            }
        };
        dbContext.Books.AddRange(books);

        await dbContext.SaveChangesAsync();
    }
}
