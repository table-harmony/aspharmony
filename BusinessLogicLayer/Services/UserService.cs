using Utils.Exceptions;
using Utils.Encryption;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;


namespace BusinessLogicLayer.Services
{
    public interface IUserService {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByCredentialsAsync(string email, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(string email, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }

    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        private readonly IEncryption _encryption;

        public UserService(IUserRepository userRepository, IEncryption encryption) {
            _userRepository = userRepository;
            _encryption = encryption;
        }

        public async Task<User> GetByIdAsync(int id) {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email) {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> GetByCredentialsAsync(string email, string password) {
            User user = await GetByEmailAsync(email);

            if (user == null)
                throw new NotFoundException();

            if (!_encryption.Compare(password, user.Password))
                throw new PublicException("Passwords do not match");

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _userRepository.GetAllAsync();
        }

        public async Task CreateAsync(string email, string password) {
            User existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
                throw new PublicException("User already exists!");

            string hashedPassword = _encryption.Encrypt(password);
            User user = new() { Email = email, Password = hashedPassword };

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user) {
            if (user == null)
                throw new ArgumentNullException();

            User existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new NotFoundException();

            existingUser.Email = user.Email;

            if (!string.IsNullOrWhiteSpace(user.Password)) {
                existingUser.Password = _encryption.Encrypt(user.Password);
            }

            await _userRepository.UpdateAsync(existingUser);
        }


        public async Task DeleteAsync(int id) {
            await _userRepository.DeleteAsync(id);
        }

    }
}
