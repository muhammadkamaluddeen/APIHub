using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Services
{
        public class WindowsAuthService
        {
            public bool Authenticate(string username, string password)
            {
                var  res = false;
                using (var context = new PrincipalContext(ContextType.Machine))
                {
                   var  re = context.ValidateCredentials(username, password);
                res = re;
                return res;
                }
            }
        }
 }

