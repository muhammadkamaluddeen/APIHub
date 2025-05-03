using APIHub.Interfaces;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using Azure;
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

            //Enqueue here so that would treat and generate UserId



            return response;

        }

        public async Task RunUpdateJob()
        {
            //select
            var response = await _userRepo.SelectUser();
            if (response != null)
            {
                foreach (var username in response)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(1000000, 10000000);

                    var rs = await _userRepo.UpdateUser(randomNumber.ToString(),username);
                }

            }

        }
    }
}
