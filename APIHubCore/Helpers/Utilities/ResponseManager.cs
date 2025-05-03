using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Helpers.Utilities
{
    public static class ResponseManager
    {
        readonly public static Tuple<string, string> Successful = new Tuple<string, string>("00", "Successful");
        readonly public static Tuple<string, string> Failed = new Tuple<string, string>("01", "Failed");
        readonly public static Tuple<string, string> Duplicate = new Tuple<string, string>("24", "Duplicate");
        readonly public static Tuple<string, string> NoRecord = new Tuple<string, string>("25", "No Record");
        readonly public static Tuple<string, string> Exception = new Tuple<string, string>("96", "Exception Error");
        readonly public static Tuple<string, string> SystemError = new Tuple<string, string>("99", "SystemError");
    }
}
