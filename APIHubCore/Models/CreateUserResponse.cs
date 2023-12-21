using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Models
{
    public class CreateUserResponse
    {
        public string? RequestId { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }
    }
}
