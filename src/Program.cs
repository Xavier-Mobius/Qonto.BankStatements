using Mobius.Qonto.API;
using Mobius.Qonto.BankStatements;
using System.CommandLine;

Console.WriteLine("Mobius.Qonto.BankStatements is running");

#region CommandLine
var secretKeyOption = new Option<string>(
        "--SecretKey",
        "You can find and manage your secret key from the Qonto web application Settings > API Key > Integrations.");
var loginOption = new Option<string>(
        "--Login",
        "You can find and manage your login identifier from the Qonto web application Settings > API Key > Integrations.");
var ibanOption = new Option<string>(
        "--IBAN",
        "The IBAN of the account.");
var monthOption = new Option<int?>(
        "--Month",
        () => null,
        "The month number (from 1 to 12) of the statement. If no month is set, the last finished month is used.");
var yearOption = new Option<int?>(
        "--Year",
        () => null,
        "The year number (eg: 2022) of the statement. If no year is set, the current year is used.");
var directoryOption = new Option<string>(
        "--Directory",
        "The directory of destination of the statement.");
var filenameOption = new Option<string?>(
        "--Filename",
        () => null,
        "The filename of the statement. If no filename is set, a default filename is generated.");

var rootCommand = new RootCommand("Mobius.Qonto.BankStatements")
{
    secretKeyOption,
    loginOption,
    ibanOption,
    monthOption,
    yearOption,
    directoryOption,
    filenameOption
};
rootCommand.TreatUnmatchedTokensAsErrors = true;
rootCommand.SetHandler(
    async (string login, string secretKey, string iban, int? month, int? year, string directory, string? filename)
        =>
    {
        await Main(login, secretKey, iban, month, year, directory, filename);
        Console.WriteLine("Mobius.Qonto.BankStatements has ending");
    },
    loginOption, secretKeyOption, ibanOption, monthOption, yearOption, directoryOption, filenameOption);

return rootCommand.Invoke(args);
#endregion

static async Task Main(string login, string secreteKey, 
    string iban, int? month, int? year,
    string directory, string? filename)
{
    using var client = new QontoClient();
    client.InitializeAuthorization(login, secreteKey);
    var (from, to) = GetDates(month, year);
    var transactions = await client.GetBankStatementAsync(iban, from, to, cancellationToken);
    var companyInfo = await client.GetCompanyInfoAsync(iban, cancellationToken);

    filename ??= $"{from:yyyy_MMMM}_{companyInfo.LegalName}_statements.xlsx";
    var serializer = new StatementSerializer(companyInfo.LegalName, companyInfo.BankAccount, from, to, transactions);
    serializer.ToExcel(Path.Combine(directory, filename));
}

static (DateTime from, DateTime to) GetDates(int? month, int? year)
{
    var parsedDate = new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1);
    if (month == null)
        parsedDate = parsedDate.AddMonths(-1);

    return (parsedDate, parsedDate.AddMonths(1).AddSeconds(-1));
}
