using APIHubCore.Helpers.Utilities;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubRepository.Repositories
{
    public class UserRepository: IUserRepo
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _logger;

         public UserRepository(IConfiguration configuration, ILoggerService logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            _logger.Info($"Request at Repo to CreateUser with request {JsonConvert.SerializeObject(request)}");

            var response = new CreateUserResponse();
            using var connection = new SqlConnection(_configuration.GetConnectionString("APIHubConnString").ToString());
            try
            { 

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", request.UserId);
                parameters.Add("@UserName", request.UserName);
                parameters.Add("@UserType", request.UserType);
                parameters.Add("@Password", request.Password);

                _logger.Info($"About to call proc_CreateUser procedure");

                var userInsertRes = connection.ExecuteScalar<string>("proc_CreateUser", parameters, commandType: System.Data.CommandType.StoredProcedure);

                _logger.Info($"Response after call to proc_CreateUser procedure is {userInsertRes}");
                if (userInsertRes == "00")
                {
                    response.ResponseCode = ResponseManager.Successful.Item1;
                    response.ResponseDescription = ResponseManager.Successful.Item2;
                }
                else
                {
                    response.ResponseCode = ResponseManager.Failed.Item1;
                    response.ResponseDescription = ResponseManager.Failed.Item2;
                }
            }
            catch(Exception ex)
            {
                response.ResponseCode = ResponseManager.Exception.Item1;
                response.ResponseDescription = ResponseManager.Exception.Item2;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return response;
        }
    }
}
