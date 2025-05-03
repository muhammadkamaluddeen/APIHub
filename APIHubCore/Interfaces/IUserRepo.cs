using APIHubCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APIHubCore.Interfaces
{
    public interface IUserRepo
    {
        public Task<CreateUserResponse> CreateUser(CreateUserRequest request);
         public Task<List<string>> SelectUser();
        public Task<CreateUserResponse> UpdateUser(string userid, string username);
        public Task<List<User>> GetUsers();


    }
}

