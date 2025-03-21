using DAL;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUserInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private static readonly Random _random = new Random();

        private readonly int needsUsersQuantity = 50;

        private static readonly string[] FirstNames =
        {
            "Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Hannah", "Isaac", "Jack",
            "Katherine", "Liam", "Mia", "Noah", "Olivia", "Paul", "Quinn", "Ryan", "Sophia", "Tom"
        };

        private static readonly string[] LastNames =
        {
            "Anderson", "Brown", "Clark", "Davis", "Evans", "Fisher", "Garcia", "Harris", "Ivers", "Jackson",
            "Kennedy", "Lopez", "Miller", "Nelson", "Owens", "Parker", "Quinn", "Roberts", "Smith", "Thompson"
        };

        public AppUserInitializer(OnlineStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void InitializeUsers()
        {
            var existingUsersCount = _context.Users.Count();

            if (existingUsersCount < needsUsersQuantity)
            {
                for (int i = 0; i < (needsUsersQuantity - existingUsersCount); i++)
                {
                    var firstName = FirstNames[_random.Next(FirstNames.Length)];
                    var lastName = LastNames[_random.Next(LastNames.Length)];

                    var user = new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = firstName,
                        LastName = lastName,
                        Age = _random.Next(18, 60),
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(1000, 9999)}@example.com",
                        UserName = $"{firstName.ToLower()}{lastName.ToLower()}{_random.Next(10, 99)}",
                        EmailConfirmed = true
                    };

                    var password = "User@123";

                    var result = _userManager.CreateAsync(user, password).Result;
                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    Console.WriteLine($"Created user: {user.Email}");
                }

                _context.SaveChanges();
            }
        }
    }
}
