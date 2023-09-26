using System.Numerics;

namespace RsaEncryption;

internal class Rsa
{
    private readonly int _publicKey;
    private readonly int _privateKey;
    private readonly int _n;

    private int[] _encryptedData;       
    private int[] _decryptedData;

    private int _progress = 0;
    private int _incrementBy = 0;

    public Rsa()
    {   
        // get keys
        var keys = new RsaKeys(100,1000);
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

    public int Crypt(int c, int key)    
    {
        return (int)(BigInteger.Pow(c, key) % _n); 
    }   

    public IReadOnlyCollection<int> EncryptTask(string message)
    {
        var tasks = new List<Task>();
        _encryptedData = new int[message.Length];  
        
        _progress = 0;
        _incrementBy = 100 / message.Length;

        for (var i = 0; i < message.Length; i++)
        {
            var index = i;  
            tasks.Add(Task.Factory.StartNew(() => EncryptData(message[index], index)));
        }

        Task.WaitAll(tasks.ToArray());

        return _encryptedData;
    }
    private void EncryptData(int c, int i)
    {
        _encryptedData[i] = Crypt(c, _publicKey);

        // progress
        _progress += _incrementBy;
        Console.WriteLine($"Encrypting: {_progress}");
    }

    public string DecryptTask(IReadOnlyCollection<int> encryptedData)
    {
        var tasks = new List<Task>();
        _decryptedData = new int[encryptedData.Count];

        for (var i = 0; i < encryptedData.Count; i++)
        {
            var index = i;
            tasks.Add(Task.Factory.StartNew(() => DecryptData(encryptedData.ElementAt(index), index)));
        }

        Task.WaitAll(tasks.ToArray());

        var decrypted = _decryptedData.Select(c => (char)c);
        return string.Join("", decrypted);
    }

    private void DecryptData(int elementAt, int index)
    {
        _decryptedData[index] = Crypt(elementAt, _privateKey);  
    }
}   