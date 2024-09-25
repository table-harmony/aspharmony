using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Initiate {

    public static class RoleInitializer {
        public static async Task InitializeAsync(IServiceProvider serviceProvider) {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Member" };

            foreach (var role in roles) {
                if (!await roleManager.RoleExistsAsync(role)) {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}