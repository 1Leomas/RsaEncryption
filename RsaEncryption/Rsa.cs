using Spectre.Console;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading.Tasks;

namespace RsaEncryption;

internal class Rsa
{
    private readonly int _publicKey;
    private readonly int _privateKey;
    private readonly int _n;

    public Rsa()
    {   
        // get keys
        var keys = new RsaKeys(1000,10000);
        _publicKey = keys.PublicKey;
        _privateKey = keys.PrivateKey;
        _n = keys.N;
    }

    public Rsa(int publicKey, int privateKey, int n)
    {
        _publicKey = publicKey;
        _privateKey = privateKey;
        _n = n;
    }

    public Rsa(RsaKeys rsaKeys)
    {
        _publicKey = rsaKeys.PublicKey;
        _privateKey = rsaKeys.PrivateKey;
        _n = rsaKeys.N;
    }

    public int Crypt(int c, int key)    
    {
        return (int)BigInteger.ModPow(c, key, _n);
    }   

    public IReadOnlyCollection<int> Encrypt(string message)
    {   
        var dataResult = new int[message.Length];

        var messageChars = new ReadOnlyCollection<int>(message.Select(x => (int)x).ToList());

        EncryptionWithTasks(messageChars, dataResult, _publicKey);

        return dataResult;
    }   
    
    public string Decrypt(IReadOnlyCollection<int> encryptedData)
    {
        var dataResult = new int[encryptedData.Count];

        EncryptionWithTasks(encryptedData, dataResult, _privateKey);

        var decrypted = dataResult.Select(c => (char)c);
        return string.Join("", decrypted);  
    }

    private void EncryptionWithTasks(IReadOnlyCollection<int> encryptedData, int[] data, int key)
    {
        var tasks = new List<Task>();
        for (var i = 0; i < encryptedData.Count; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                data[index] = Crypt(encryptedData.ElementAt(index), key);
            }));
        }
        Task.WaitAll(tasks.ToArray());
    }
}   