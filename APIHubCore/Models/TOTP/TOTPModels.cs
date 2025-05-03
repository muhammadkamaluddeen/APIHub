using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Models.TOTP
{
    public class TOTPModels
    {
        public class TotpRequest
        {
            public string Key { get; set; }
            public int Step { get; set; } = 30;
            public int Digits { get; set; } = 6;
        }

        public class TotpValidationRequest
        {
            public string Key { get; set; }
            public string Token { get; set; }
            public int Step { get; set; } = 30;
            public int Digits { get; set; } = 6;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
