using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using static APIHubCore.Models.TOTP.TOTPModels;

namespace APIHub.Controllers
{
    [Route("Api/controller")]
    [ApiController]
    public class TOTPController : ControllerBase
    {

        [HttpPost("generate")]
        public ActionResult<string> GenerateTotp([FromBody] TotpRequest request)
        {
            var key = Base32Encoding.ToBytes(request.Key);
            var totp = new Totp(key, mode: OtpHashMode.Sha1, step: request.Step, totpSize: request.Digits);
            var token = totp.ComputeTotp();
            return Ok(token);
        }

        [HttpPost("validate")]
        public ActionResult<bool> ValidateTotp([FromBody] TotpValidationRequest request)
        {
            var key = Base32Encoding.ToBytes(request.Key);
            var totp = new Totp(key, mode: OtpHashMode.Sha1, step: request.Step, totpSize: request.Digits);
            var isValid = totp.VerifyTotp(request.Token, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
            return Ok(isValid);
        }
    }
}
