using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Models
{
    public class User
    {
            public string Username { get; set; }
            public string Fullname { get; set; }
            public string UserId { get; set; }        
    }
}
