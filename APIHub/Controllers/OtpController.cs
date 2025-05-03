using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly OtpService _otpService;

    public OtpController(OtpService otpService)
    {
        _otpService = otpService;
    }

    // POST: api/otp/generate
    [HttpPost("generate")]
    public IActionResult GenerateOtp([FromBody] OtpRequest request)
    {
        var (isAllowed, otp, message) = _otpService.GenerateOtp(request.EmailOrPhone);

        if (!isAllowed)
        {
            return BadRequest(new { Message = message });
        }

        return Ok(new { Otp = otp, Message = message });
    }

    // POST: api/otp/validate
    [HttpPost("validate")]
    public IActionResult ValidateOtp([FromBody] OtpValidationRequest request)
    {
        bool isValid = _otpService.ValidateOtp(request.EmailOrPhone, request.Otp);
        if (isValid)
        {
            return Ok(new { Message = "OTP validated successfully" });
        }
        else
        {
            return BadRequest(new { Message = "Invalid or expired OTP" });
        }
    }
}

// Request DTOs
public class OtpRequest
{
    public string EmailOrPhone { get; set; }
}

public class OtpValidationRequest
{
    public string EmailOrPhone { get; set; }
    public string Otp { get; set; }
}
