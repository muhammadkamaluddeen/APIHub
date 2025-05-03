using APIHubCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using static APIHubCore.Models.TOTP.TOTPModels;
using LoginRequest = APIHubCore.Models.TOTP.TOTPModels.LoginRequest;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.IO;

namespace APIHub.Controllers
{
    [Route("Api/controller")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly WindowsAuthService _windowsAuthService;

        public LoginController(WindowsAuthService windowsAuthService)
        {
            _windowsAuthService = windowsAuthService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            var con = new PdfService();

            con.ConvertTo();









            //        string htmlContent = @"
            //                <html>
            //                <head>
            //                    <title>Sample PDF</title>
            //                    <style>
            //                        body { font-family: Arial, sans-serif; }
            //                        h1 { color: blue; }
            //                        p { font-size: 14px; }
            //                    </style>
            //                </head>
            //                <body>
            //                    <h1>Hello, World!</h1>
            //                    <p>This is a sample PDF generated from HTML content.</p>
            //                     <table>
            //                      <thead>
            //                         <th>Id</th>
            // <th>Firstname</th>

            // <th>Lastname</th>


            //<thead>

            //<tbody>
            //  <tr>
            //   <td>1</td>
            //   <td>Muhammad Kamal</td>
            //   <td>Mahmood</td>
            //</tr>

            // <tr>
            //   <td>1</td>
            //   <td>Muhammad Kamal</td>
            //   <td>Mahmood</td>
            //</tr>

            // <tr>
            //   <td>1</td>
            //   <td>Muhammad Kamal</td>
            //   <td>Mahmood</td>
            //</tr>

            //</tbody>

            //</table>

            //                </body>
            //                </html>";

            //            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Sample.pdf");

            //            PdfService converter = new PdfService();
            //            converter.ConvertHtmlToPdf(htmlContent, outputPath);

            //            Console.WriteLine($"PDF has been saved to: {outputPath}");



            var res = _windowsAuthService.Authenticate(request.Username, request.Password);

            var result = res;
            if (res)
            {
                return Ok(new { message = "Authentication successful" });
            }
            else
            {
                return Ok(new { message = "Authentication Failed" });
            }
           

        }
    }
}
