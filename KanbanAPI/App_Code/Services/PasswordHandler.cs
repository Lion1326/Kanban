using System.Security.Cryptography;
using System.Text;

public class PasswordHandler
{
    public static string GetHashString(string str, string key)
    {
        DateTime date = DateTime.UtcNow;
        StringBuilder sb = new StringBuilder();
        byte[] hashStr = GetHash(str);
        byte[] hashKey = GetHash(key);
        foreach (byte b in GenerateSaltedHash(hashStr, hashKey))
            sb.Append(b.ToString("X2"));
        return sb.ToString();
    }

    static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
    {
#pragma warning disable SYSLIB0021 // Type or member is obsolete
        HashAlgorithm algorithm = new SHA256Managed();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
        byte[] plainTextWithSaltBytes =
          new byte[plainText.Length + salt.Length];
        for (int i = 0; i < plainText.Length; i++)
        {
            plainTextWithSaltBytes[i] = plainText[i];
        }
        for (int i = 0; i < salt.Length; i++)
        {
            plainTextWithSaltBytes[plainText.Length + i] = salt[i];
        }
        return algorithm.ComputeHash(plainTextWithSaltBytes);
    }

    public static byte[] GetHash(string inputString)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
}


