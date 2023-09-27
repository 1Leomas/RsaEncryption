using RsaEncryption;
using Spectre.Console;

while (true)
{
    Console.Clear();

    var rsaKeys = new RsaKeys(100, 500);
    var rsa = new Rsa(rsaKeys);

    PrintRsaKeyValues(rsaKeys);

    Console.Write("Message to encrypt: ");
    var input = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(input)) continue;

    var encrypted = rsa.Encrypt(input);
    var decrypted = rsa.Decrypt(encrypted);

    if (input.Length < 20)
    {
        PrintMessageInNumbers(input);
        PrintEncryptedMessage(encrypted);
    }

    AnsiConsole.Write(new Markup(
        $"[red]Message decrypted: [/]{decrypted}\n"
    ));

    AnsiConsole.Write(new Markup(
        "\n[green]Press any key to continue[/] or [red]'q'[/] to quit"
    ));

    if (Console.ReadKey().KeyChar == 'q') break;
}

return;

void PrintRsaKeyValues(RsaKeys rsaKeys1)
{
    Console.SetWindowSize(60, 90);
    var rule = new Rule("[red]RSA[/]").RuleStyle("red bold");
    AnsiConsole.Write(rule);
    var table = new Table();
    table.Border = TableBorder.Heavy;

    table.AddColumn(new TableColumn("P").Centered());
    table.AddColumn(new TableColumn("Q").Centered());
    table.AddColumn(new TableColumn("N").Centered());
    table.AddColumn(new TableColumn("PHI").Centered());
    table.AddColumn(new TableColumn("E").Centered());
    table.AddColumn(new TableColumn("D").Centered());

    table.AddRow($"{rsaKeys1.P}", $"{rsaKeys1.Q}", $"{rsaKeys1.N}", $"{rsaKeys1.Phi}", $"{rsaKeys1.E}",
        $"{rsaKeys1.D}");

    AnsiConsole.Write(table);
}

void PrintEncryptedMessage(IReadOnlyCollection<int> readOnlyCollection)
{
    AnsiConsole.Write(new Markup(
        "[green]Message encrypted: [/]"
    ));
    foreach (var c in readOnlyCollection)
        Console.Write($"{c} ");
    Console.WriteLine();
}

void PrintMessageInNumbers(string s)
{
    Console.Write("Message in numbers: ");
    foreach (var c in s)
        Console.Write($"{(int)c} ");
    Console.WriteLine();
}
