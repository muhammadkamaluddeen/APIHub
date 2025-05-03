using APIHubCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _loggerService;
        public LoggerService(ILogger<LoggerService> loggerService)
        {

            _loggerService = loggerService;

        }
        public void Info(string message)
        {
            _loggerService.LogInformation(message);
        }
    }
}
