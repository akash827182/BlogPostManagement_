using AutoMapper;

using Q3.Business.IBusinessServices;
using Q3.Data.Entities;
using Q3.Data.IRepository;
using Q3.Shared.DTO.MainData;
using Q3.Shared.Exceptions;
using Q3.Shared.Interfaces;


namespace Q3.Business.BusinessServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task RegisterUserAsync(UserDto userDto)
        {

            var existingUser = await _userRepository.GetByUsernameAsync(userDto.Username);
            if (existingUser != null)
            {
                throw new UsernameAlreadyExistsException();

            }

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role=userDto.Role
            };
            await _userRepository.AddAsync(user);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty");
            }

            var user = await _userRepository.GetByUsernameAsync(username);
            return user;
        }



        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(static u => new UserDto
            {
                Id=u.Id,
                Username = u.Username,
                Password = null,
                Role=u.Role

            });
        }

        public async Task<string> AuthenticateAndGenerateToken(UserDto userDto)
        {
            var user = await _userRepository.GetByUsernameAsync(userDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var userDtoForToken = _mapper.Map<UserDto>(user);

            return _tokenService.GenerateJwtToken(userDtoForToken);
        }
    }
}
