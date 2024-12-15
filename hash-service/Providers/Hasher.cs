using System;
using System.Security.Cryptography;
using System.Text;

public class Hasher
{
    public static string ComputeHash(
           string data,
           hash_service.Algorithms algorithm,
           int iterations = 1,
           hash_service.Format outputFormat = hash_service.Format.HEX)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        HashAlgorithm hashAlgorithm = algorithm switch
        {
            hash_service.Algorithms.SHA256 => SHA256.Create(),
            hash_service.Algorithms.SHA1 => SHA1.Create(),
            hash_service.Algorithms.MD5 => MD5.Create(),
            _ => throw new ArgumentException("Unsupported algorithm: " + algorithm)
        };

        byte[] hashBytes = dataBytes;
        for (int i = 0; i < iterations; i++)
        {
            hashBytes = hashAlgorithm.ComputeHash(hashBytes);
        }

        return outputFormat switch
        {
            hash_service.Format.HEX => BitConverter.ToString(hashBytes).Replace("-", "").ToLower(),
            hash_service.Format.BASE64 => Convert.ToBase64String(hashBytes),
            _ => throw new ArgumentException("Unsupported output format: " + outputFormat)
        };
    }
}