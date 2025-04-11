using DAL;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUserInitializer
    {
        private readonly OnlineStoreDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private static readonly Random _random = new();

        private const int NEEDS_USER_QUANTITY = 50;

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

        public void InitializeUsersAsync()
        {
            var existingUsersCount = _context.AppUsers.Count();

            if (existingUsersCount < NEEDS_USER_QUANTITY)
            {

                for (int i = 0; i < (NEEDS_USER_QUANTITY - existingUsersCount); i++)
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

                    _userManager.CreateAsync(user, password);

                }

                _context.SaveChanges();
            }
        }
    }
}
