using RsaEncryption;
using Spectre.Console;

var rsa = new Rsa();

Console.Write("Input: ");
var input = Console.ReadLine();

foreach (var c in input)
    Console.Write($"{(int)c} ");
Console.WriteLine();

var encrypted = rsa.EncryptTask(input);

Console.WriteLine("\n\nEncrypted:");
foreach (var c in encrypted)
    Console.Write($"{c} ");

var decrypted = rsa.DecryptTask(encrypted);

Console.WriteLine($"\n\nDecrypted: {decrypted}");
Console.ReadLine();

return;
