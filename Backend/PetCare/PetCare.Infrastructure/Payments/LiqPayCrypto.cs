namespace PetCare.Infrastructure.Payments;

using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides cryptographic utility methods for encoding data in Base64 and generating SHA-1 signatures for use with
/// LiqPay API operations.
/// </summary>
/// <remarks>This class is intended for internal use when interacting with the LiqPay payment system. All methods
/// are static and thread-safe. The signature generation method follows the LiqPay protocol, which requires
/// concatenating the private key and data before hashing. These utilities should not be used for general-purpose
/// cryptography outside the LiqPay context.</remarks>
internal static class LiqPayCrypto
{
    /// <summary>
    /// Encodes the specified string into its Base64 representation using UTF-8 encoding.
    /// </summary>
    /// <param name="s">The string to encode. If <paramref name="s"/> is null or empty, the result will be the Base64 encoding of an
    /// empty string.</param>
    /// <returns>A Base64-encoded string that represents the input value.</returns>
    public static string Base64(string s) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(s));

    /// <summary>
    /// Generates a SHA-1 signature for the specified data using the provided private key.
    /// </summary>
    /// <remarks>The signature is computed by concatenating the private key, the Base64-encoded data, and the
    /// private key again, then hashing the result with SHA-1. This method does not validate the format of the input
    /// data; callers must ensure that the data is properly Base64-encoded. The method is static and
    /// thread-safe.</remarks>
    /// <param name="privateKey">The private key used to generate the signature. Cannot be null.</param>
    /// <param name="dataBase64">The data to sign, encoded as a Base64 string. Cannot be null.</param>
    /// <returns>A Base64-encoded string representing the SHA-1 signature of the input data and private key.</returns>
    public static string Sign(string privateKey, string dataBase64)
    {
        // signature = base64( sha1(private_key + data + private_key) )
        using var sha1 = SHA1.Create();
        var bytes = Encoding.UTF8.GetBytes(privateKey + dataBase64 + privateKey);
        var hash = sha1.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
