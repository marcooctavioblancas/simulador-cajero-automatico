using System.Security.Cryptography;
using System.Text;

namespace BankIA.Helpers;

internal static class SecurityHelper
{
    /// <summary>SHA-256 del NIP → hex uppercase (64 chars). Usado por BankIA.</summary>
    public static string HashNIP(string nip)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(nip));
        return Convert.ToHexString(bytes); // 64 chars
    }

    /// <summary>SHA-512 del NIP → hex uppercase (128 chars). Usado por Banco2.</summary>
    public static string HashNIPSHA512(string nip)
    {
        byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(nip));
        return Convert.ToHexString(bytes); // 128 chars
    }

    /// <summary>
    /// Verifica el NIP contra el hash almacenado.
    /// Auto-detecta el algoritmo por la longitud del hash:
    ///   64 chars  → SHA-256 (BankIA)
    ///   128 chars → SHA-512 (Banco2 u otro banco externo)
    /// </summary>
    public static bool VerificarNIP(string nipIngresado, string hashAlmacenado)
    {
        string hashCalculado = hashAlmacenado.Length <= 64
            ? HashNIP(nipIngresado)       // SHA-256
            : HashNIPSHA512(nipIngresado); // SHA-512

        return string.Equals(hashCalculado, hashAlmacenado, StringComparison.OrdinalIgnoreCase);
    }
}
