using APIHubCore.Helpers.Utilities;
using APIHubCore.Interfaces;
using APIHubCore.Models;
using Azure.Core;
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
                parameters.Add("@UserName", request.Username);
                parameters.Add("@Fullname", request.Fullname);

                _logger.Info($"About to call InsertUser procedure");

                var userInsertRes = connection.ExecuteScalar<string>("InsertUser", parameters, commandType: System.Data.CommandType.StoredProcedure);

                _logger.Info($"Response after call to InsertUser procedure is {userInsertRes}");
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

        public async  Task<List<User>> GetUsers()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("APIHubConnString").ToString());
            try
            {

                IEnumerable<User> users = connection.Query<User>("proc_GetUsers", commandType: System.Data.CommandType.StoredProcedure);

                var response = new List<User>();    

                foreach (var user in users)
                {
                    response.Add(user); 
                }

                return response; 
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return null;
        }

        public async Task<List<string>> SelectUser()
        {
           
            using var connection = new SqlConnection(_configuration.GetConnectionString("APIHubConnString").ToString());
            try
            {

                IEnumerable<string> usernames = connection.Query<string>("SelectUser", commandType: System.Data.CommandType.StoredProcedure);
        
                if (usernames != null)
                {

                    var userNameList = usernames.ToList();
                    return userNameList;

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return null;
        }

        public async Task<CreateUserResponse> UpdateUser(string userid, string username)

        {

          var response = new CreateUserResponse();
            using var connection = new SqlConnection(_configuration.GetConnectionString("APIHubConnString").ToString());
            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userid);
                parameters.Add("@UserName", username);



                var userUpdateRes = connection.ExecuteScalar<string>("UpdateUser", parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (userUpdateRes == "00")
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
            catch (Exception ex)
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
