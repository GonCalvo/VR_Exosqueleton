using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class Encryptor
{

    private const string pepper = "1012$";

    public static int DEFAULT_COST = 16;

    private const int SIZE = 128;

    private int cost;

    public Encryptor()
    {
        this.cost = DEFAULT_COST;
    }

    public Encryptor(int cost)
    {
        this.cost = cost;
    }

    public string hash(string s)
    {
        using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[SIZE / 8];
            crypto.GetBytes(salt);

            byte[] dk = pbkdf2(s, salt, 1 << cost);
            byte[] hash = new byte[salt.Length + dk.Length];

            Array.Copy(salt, 0, hash, 0, salt.Length);
            Array.Copy(dk, 0, hash, salt.Length, dk.Length);
            return pepper + cost + '$' + Convert.ToBase64String(hash);

        }
    }

    public string encode(string name)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(name));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }


    private int iterations(int cost)
    {
        if ((cost < 0) || (cost > 30))
            throw new ArgumentOutOfRangeException("cost: " + cost);
        return 1 << cost;
    }

    public bool authenticate(string password, string token)
    {
        var parts = token.Split('$');
        if (parts.Length != 3)
        {
            throw new FormatException("Token \"" + token + "\" not properly formatted");
        }

        int iterations = this.iterations(int.Parse(parts[1]));

        Console.WriteLine("[DEBUG]: iterations value = " + iterations);
        Console.WriteLine("[DEBUG]: salt+pw = " + parts[2]);
        byte[] hash = Convert.FromBase64String(parts[2]);
        int len = SIZE / 8;
        byte[] salt = new byte[len];
        Array.Copy(hash, 0, salt, 0, len);
        byte[] check = pbkdf2(password, salt, iterations);
        int zero = 0;
        for (int idx = 0; idx < check.Length; ++idx)
            zero |= hash[salt.Length + idx] ^ check[idx];
        return zero == 0;
    }

    private byte[] pbkdf2(string s, byte[] salt, int iterations)
    {
        Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(s, salt, iterations);
        return k1.GetBytes(SIZE);
    }


}
