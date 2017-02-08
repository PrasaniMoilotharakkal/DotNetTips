
using System;
using System.Security.Cryptography;
using System.Text;

public class Harish
{

    static void Main(string[] args)
    {
        var ss = CreateKeyPair();
        Console.ReadLine();
    }

    public static Tuple<string, string> CreateKeyPair()
    {
        CspParameters cspParams = new CspParameters
        {
            ProviderType = 1 /* PROV_RSA_FULL */
        };

        RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048, cspParams);

        string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));
        string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));

        Encrypt(publicKey, privateKey);

        return new Tuple<string, string>(privateKey, publicKey);
    }

    private static void Encrypt(string publicKey, string privateKey)
    {

        string str = "This is some text for me to encrypt, it can't be too long.";
        CspParameters cspParams = new CspParameters
        {
            ProviderType = 1 /* PROV_RSA_FULL */
        };

        RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048, cspParams);

        rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));

        var unencryptedFile = Encoding.ASCII.GetBytes(str);

        var encryptedFile = rsaProvider.Encrypt(unencryptedFile, true);
        Console.WriteLine("Original:" + str);
        Console.WriteLine("Encrypted:" + encryptedFile);
        
        Decrypt(privateKey, encryptedFile);       
    }

    private static void Decrypt(string privateKey, byte[] encryptedFile)
    {
        CspParameters cspParams =
           new CspParameters
           {
               ProviderType = 1 /* PROV_RSA_FULL */
           };

        RSACryptoServiceProvider rsaProvider =
            new RSACryptoServiceProvider(2048, cspParams);

        rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));

        string unencryptedFileString = Encoding.ASCII.GetString(rsaProvider.Decrypt(encryptedFile, true));

        Console.WriteLine("After Decrypt:" + unencryptedFileString);
    }
}