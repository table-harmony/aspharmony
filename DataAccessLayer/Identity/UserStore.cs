using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;

namespace DataAccessLayer.Identity {
    public class UserStore(
        ApplicationContext context,
        IUserRepository userRepository,
        IdentityErrorDescriber? describer = null) : UserStore<User>(context, describer) {

        public override async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default) {
            try {
                await userRepository.DeleteAsync(user.Id);
                return IdentityResult.Success;
            }
            catch (Exception ex) {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
    }
} 