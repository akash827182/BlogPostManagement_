
using Q3.Shared.DTO.MainData;

namespace Q3.Shared.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserDto userDto);
    }
}
