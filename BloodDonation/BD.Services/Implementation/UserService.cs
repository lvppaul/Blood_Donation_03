using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<UserResponse> AddAsync(UserRequest user)
        {
            var entity = UserMapper.ToEntity(user);
            await _repository.AddAsync(entity);
            return UserMapper.ToResponse(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            await _repository.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(UserMapper.ToResponse);
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : UserMapper.ToResponse(user);
        }

        public async Task<UserResponse?> UpdateAsync(int userId, UserRequest user)
        {
            var userFound = await _repository.GetByIdAsync(userId);
            if (userFound != null)
            {
                userFound.Name = user.Name;
                userFound.BloodType = user.BloodType;
                userFound.Phone = user.Phone;
                userFound.Address = user.Address;
                userFound.Email = user.Email;
                await _repository.UpdateAsync(userFound);
                return UserMapper.ToResponse(userFound);
            }
            return null;
        }

        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            return user == null ? null : UserMapper.ToResponse(user);
        }

        public async Task<UserResponse?> GetByPhoneAsync(string phone)
        {
            var user = await _repository.GetByPhoneAsync(phone);
            return user == null ? null : UserMapper.ToResponse(user);
        }
    }
}
