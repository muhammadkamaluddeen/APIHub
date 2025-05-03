using Azure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Models
{
    public class CreateUserRequest
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$")]
        public string RequestId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$")]
        public string Fullname { get; set; }


    }

    
}
