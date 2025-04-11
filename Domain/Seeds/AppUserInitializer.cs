using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.UserService
{
    public class UserInitializationService
    {
        private readonly OnlineStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private static readonly Random _random = new();

        private const int NeedsUsersQuantity = 50;

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

        public UserInitializationService(OnlineStoreDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task InitializeUsersAsync()
        {
            var existingUsersCount = await _context.Users.CountAsync();

            if (existingUsersCount >= NeedsUsersQuantity)
            {
                Console.WriteLine("🟡 Достаточное количество пользователей уже существует.");
                return;
            }

            var usersToCreate = NeedsUsersQuantity - existingUsersCount;

            for (int i = 0; i < usersToCreate; i++)
            {
                var firstName = FirstNames[_random.Next(FirstNames.Length)];
                var lastName = LastNames[_random.Next(LastNames.Length)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(1000, 9999)}@example.com";
                var username = $"{firstName.ToLower()}{lastName.ToLower()}{_random.Next(10, 99)}";

                var user = new AppUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = _random.Next(18, 60),
                    Email = email,
                    UserName = username,
                    EmailConfirmed = true
                };

                var password = "User@123";

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"❌ Ошибка при создании пользователя {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    continue;
                }

                Console.WriteLine($"✅ Создан пользователь: {email}");
            }
        }
    }
}
