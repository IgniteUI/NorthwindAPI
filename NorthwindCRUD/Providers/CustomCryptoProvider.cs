using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace NorthwindCRUD.Providers;

/// <summary>
/// Custom Crypto Provider for HMAC-SHA512 algorithm which ignores the minimum key size requirement to allow the previous keys to work.
/// </summary>
public class HmacSha512CryptoProvider : ICryptoProvider
{
    public object Create(string algorithm, params object[] args)
    {
        if (!IsSupportedAlgorithm(algorithm, args))
        {
            throw new NotSupportedException("Algorithm not supported.");
        }

        var keyBytes = (args != null && args.Length > 0 && args[0] is byte[] bytes ? bytes : null)
            ?? throw new ArgumentNullException(nameof(args), "Key bytes must be provided as the first argument.");

        if (keyBytes.Length < 28)
        {
            throw new CryptographicException("The key size is too small for HMAC-SHA512. Minimum key size is 28 bytes.");
        }

        return new HMACSHA512(keyBytes);
    }

    // Indicate supported algorithms
    public bool IsSupportedAlgorithm(string algorithm, params object[] args)
    {
        if (string.IsNullOrEmpty(algorithm))
        {
            return false;
        }

        var keyBytes = args != null && args.Length > 0 && args[0] is byte[] bytes ? bytes : null;
        if (keyBytes == null || keyBytes.Length == 0)
        {
            // We only care about handling keybytes for HmacSha512
            return false;
        }

        // Accept common HMAC-SHA512 names (including the constant)
        return string.Equals(algorithm, SecurityAlgorithms.HmacSha512, StringComparison.OrdinalIgnoreCase)
                || algorithm.Contains("HmacSha512", StringComparison.OrdinalIgnoreCase);
    }

    // Release/dispose a created crypto instance
    public void Release(object cryptoInstance)
    {
        if (cryptoInstance is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
