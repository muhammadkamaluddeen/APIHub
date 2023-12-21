using APIHub.Interfaces;
using APIHubCore.Helpers.Utilities;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace APIHub.Controllers
{
    [Route("Api/controller")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly ILoggerService _logger;
        public UserController(IUser user, ILoggerService logger)
        {
            _user = user;
            _logger = logger;
        }

       [HttpPost]
       
        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
        {
            _logger.Info($"Request received: {JsonConvert.SerializeObject(request)}");
           var response = new CreateUserResponse();

            _logger.Info($"About to CreateUser with request: {JsonConvert.SerializeObject(request)}");
            response = await _user.CreateUser(request);
            _logger.Info($"Response received: {JsonConvert.SerializeObject(response)}");
            response.RequestId = request.RequestId;

            _logger.Info($"Final response returned: {JsonConvert.SerializeObject(response)}");
            return Ok(response);
        
        }
    }
}
