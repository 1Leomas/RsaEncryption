using System.Numerics;

namespace RsaEncryption;

public sealed class RsaKeys
{
    private readonly int _min = 100;
    private readonly int _max = 1000;

    public int P;
    public int Q;
    public int N;
    public int Phi;
    public int E;
    public int D;   

    public int PublicKey => E;
    public int PrivateKey => D;

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
        P = GetPrimeNumber();
        Q = GetPrimeNumber();

        while (P == Q)
        {
            P = GetPrimeNumber();
            Q = GetPrimeNumber();
        }

        // (2)
        N = P * Q;
        // (3)
        Phi = (P - 1) * (Q - 1);
        // (4)
        E = GetE(Phi);
        // (5)
        D = GetD(E, Phi);
    }

    int GetD(int e, int phi)
    {
        BigInteger d = 1;
        while (true)
        {
            // cautam asa un d astfel incat d * e % phi == 1
            if (d * e % phi == 1)
                return (int)d;
            d++;
        }
    }

    int GetE(int phi)
    {
        // 1 < e < phi
        var e = GetRandomNumber(2, phi);

        // e si phi trebuie sa fie coprime
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