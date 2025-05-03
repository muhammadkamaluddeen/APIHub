using OtpNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class OtpService
{
    private readonly ConcurrentDictionary<string, List<DateTime>> _otpRequestTimestamps;
    private readonly ConcurrentDictionary<string, string> _usedOtps;
    private readonly ConcurrentDictionary<string, Totp> _activeOtps;
    private readonly byte[] _secretKey;

    public OtpService()
    {
        _secretKey = KeyGeneration.GenerateRandomKey(20);
        _usedOtps = new ConcurrentDictionary<string, string>();
        _activeOtps = new ConcurrentDictionary<string, Totp>();
        _otpRequestTimestamps = new ConcurrentDictionary<string, List<DateTime>>(); // Ensuring correct initialization here
    }

    // Generate OTP with rate limiting (3 requests in 5 minutes)
    public (bool IsAllowed, string Otp, string Message) GenerateOtp(string emailOrPhone)
    {
        if (!IsAllowedToRequestOtp(emailOrPhone))
        {
            return (false, null, "Rate limit exceeded. Please wait before requesting another OTP.");
        }

        var totp = new Totp(_secretKey, step: 60);
        string otp = totp.ComputeTotp(DateTime.UtcNow);

        _activeOtps[emailOrPhone] = totp;
        RecordOtpRequest(emailOrPhone);

        return (true, otp, "OTP generated successfully.");
    }

    // Validate OTP based on a unique identifier
    public bool ValidateOtp(string emailOrPhone, string otp)
    {
        if (_usedOtps.ContainsKey(emailOrPhone) && _usedOtps[emailOrPhone] == otp)
        {
            return false; // OTP already used
        }

        if (_activeOtps.TryGetValue(emailOrPhone, out Totp totp))
        {
            bool isValid = totp.VerifyTotp(otp, out long timeStepMatched);

            if (isValid)
            {
                _usedOtps[emailOrPhone] = otp;
                _activeOtps.TryRemove(emailOrPhone, out _); // Remove the used OTP
            }

            return isValid;
        }

        return false;
    }

    // Check if a user is allowed to request an OTP (rate limiting)
    private bool IsAllowedToRequestOtp(string emailOrPhone)
    {
        var now = DateTime.UtcNow;

        // Ensure _otpRequestTimestamps is not null (this should be ensured by the constructor)
        if (_otpRequestTimestamps == null)
        {
            throw new NullReferenceException("OTP request timestamps dictionary is not initialized.");
        }

        // Check if the user has made previous requests
        if (_otpRequestTimestamps.TryGetValue(emailOrPhone, out List<DateTime> timestamps))
        {
            // Remove timestamps older than 5 minutes
            timestamps = timestamps?.Where(t => t > now.AddMinutes(-5)).ToList();

            // Update the list after removing old timestamps
            _otpRequestTimestamps[emailOrPhone] = timestamps;

            // Check if they have reached the rate limit (3 requests in 5 minutes)
            if (timestamps.Count >= 3)
            {
                return false; // Rate limit exceeded
            }
        }

        return true;
    }

    // Record a new OTP request timestamp
    private void RecordOtpRequest(string emailOrPhone)
    {
        var now = DateTime.UtcNow;

        // Add the current timestamp for the user
        if (_otpRequestTimestamps.TryGetValue(emailOrPhone, out List<DateTime> timestamps))
        {
            timestamps.Add(now);
        }
        else
        {
            _otpRequestTimestamps[emailOrPhone] = new List<DateTime> { now };
        }
    }
}
