namespace RsaEncryption;

public sealed class RsaKeys
{
    private readonly int _min = 100;
    private readonly int _max = 1000;

    private int _p;
    private int _q; 
    private int _n;
    private int _phi;
    private int _e;
    private int _d;

    public int PublicKey => _e;
    public int PrivateKey => _d;
    public int N => _n;

    public RsaKeys()
    {   
        Generate();
    }

    public RsaKeys(int min, int max)
    {
        (_min, _max) = (min, max);
        Generate();
    }

    public void Generate()
    {   
        // (1)
        _p = GetPrimeNumber();
        _q = GetPrimeNumber();

        while (_p == _q)
        {
            _p = GetPrimeNumber();
            _q = GetPrimeNumber();
        }

        Console.WriteLine($"p = {_p}");
        Console.WriteLine($"q = {_q}");

        // (2)
        _n = _p * _q;
        Console.WriteLine($"n = {_n}");

        // (3)
        _phi = (_p - 1) * (_q - 1);
        Console.WriteLine($"phi = {_phi}");

        // (4)
        _e = GetE(_phi);
        Console.WriteLine($"e = {_e}");

        // (5)
        _d = GetD(_e, _phi);
        Console.WriteLine($"d = {_d}");
    }

    int GetD(int e, int phi)
    {
        var d = 1;
        while (true)
        {
            if (d * e % phi == 1)
                return d;
            d++;
        }
    }

    int GetE(int phi)
    {
        var e = GetRandomNumber(2, phi);

        while (Cmmdc(e, phi) != 1)
            e = GetRandomNumber(2, phi);

        return e;
    }   

    static int Cmmdc(int e, int phi)
    {
        while (e != phi)
        {
            if (e > phi)
                e -= phi;
            else
                phi -= e;
        }
        return e;
    }


        
    int GetPrimeNumber()
    {
        var number = GetRandomNumber(_min, _max);
        while (!IsPrimeNumber(number))
            number = GetRandomNumber(_min, _max);
        return number;
    }

    static int GetRandomNumber(int min, int max) => new Random().Next(min, max);

    static bool IsPrimeNumber(int n)
    {
        if (n <= 1) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;
        for (var i = 3; i < n; i += 2)
            if (n % i == 0) return false;
        return true;
    }
}