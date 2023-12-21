using APIHubCore.Models;

namespace APIHub.Interfaces
{
    public interface IUser
    {
        public Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        
    }
}
