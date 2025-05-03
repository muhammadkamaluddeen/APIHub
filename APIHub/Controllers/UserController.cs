using APIHub.Interfaces;
using APIHubCore.Helpers.Utilities;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using APIHubCore.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OtpNet;
using System.Text.Json.Serialization;

namespace APIHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly ILoggerService _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IUserRepo _userRepo;
        private readonly IRedisCacheService _redisCacheService;



        public UserController(IUser user, ILoggerService logger, IBackgroundJobClient backgroundJobClient, IUserRepo userRepo, IRedisCacheService redisCacheService)
        {
            _user = user;
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
            _userRepo = userRepo;
            _redisCacheService = redisCacheService;

        }

        [HttpPost("create-user")]

        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
        {

            var arrayStr = new Response[3]
   {
            new Response { Id = 1, Name = "CBN" },
            new Response { Id = 2, Name = "FIRS" },
            new Response { Id = 3, Name = "NNPC" }
   };


                var key = KeyGeneration.GenerateRandomKey(20);
                var fkey =  Base32Encoding.ToString(key);



            // var res = _myService.GetCacheItem("dropKey");


            // _myService.SetCacheItem("dropKey", arrayStr);

            _logger.Info($"Request received: {JsonConvert.SerializeObject(request)}");
            var response = new CreateUserResponse();

            _logger.Info($"About to CreateUser with request: {JsonConvert.SerializeObject(request)}");
            response = await _user.CreateUser(request);
            _logger.Info($"Response received: {JsonConvert.SerializeObject(response)}");
            response.RequestId = request.RequestId;
            _logger.Info($"Final response returned: {JsonConvert.SerializeObject(response)}");


            // Schedule a recurring job to run every 5 seconds
            RecurringJob.AddOrUpdate(() => _user.RunUpdateJob(), "* * * * * *");


            return Ok(response);
        
        }


        [HttpGet("get-users")]
        public async Task<ActionResult<User>> SelectUser()
        {

            var cacheKey = "UsersCache";

            // Try to get the user from Redis cache
            var users = await _redisCacheService.GetCacheValueAsync<List<User>>(cacheKey);
            if (users != null)
            {
                return Ok(users); // Return user from cache
            }

            // If not in cache, get from DB and store in Redis cache
            users = await _userRepo.GetUsers();
            if (users == null)
            {
                return NotFound();
            }

            // Store user in cache for 10 minutes
            await _redisCacheService.SetCacheValueAsync(cacheKey, users);


            return Ok(users);

        }
    }
}
