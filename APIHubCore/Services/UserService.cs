using APIHub.Interfaces;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Services
{
    public class UserService : IUser
    {
        private readonly IUserRepo _userRepo;
        private readonly ILoggerService _logger;
        public UserService(IUserRepo userRepo,ILoggerService logger)
        {
            _userRepo = userRepo;   
            _logger = logger;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var response = new CreateUserResponse();

            _logger.Info($"Request received at Core to CreateUser with request {JsonConvert.SerializeObject(request)}");
            response = await _userRepo.CreateUser(request);
            _logger.Info($"Response received at Core CreateUser: {JsonConvert.SerializeObject(response)}");


            return response;

        }
    }
}
