using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
    }
}
