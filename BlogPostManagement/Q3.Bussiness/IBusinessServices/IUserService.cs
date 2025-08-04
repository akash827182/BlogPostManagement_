using Q3.Data.Entities;
using Q3.Shared.DTO.MainData;

namespace Q3.Business.IBusinessServices
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task RegisterUserAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<string> AuthenticateAndGenerateToken(UserDto userDto);
    }
}
