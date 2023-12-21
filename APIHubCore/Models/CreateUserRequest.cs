using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Models
{
    public class CreateUserRequest
      {
            public string ?RequestId { get; set; }
            public string UserId { get; set; }
            public string? UserName { get; set; }
            public string? UserType { get; set; }
            public string? Password { get; set; }
        }
    }

