using Mobius.Qonto.BankStatements.Model;
using System.Text.Json;
using System.Linq;

namespace Mobius.Qonto.BankStatements;

public class QontoClient : IDisposable
{
    private const string K_QONTO_API_BASE_ADDRESS = "https://thirdparty.qonto.com/";

    private HttpClient? _HttpClient;
    private bool disposedValue;

    public HttpClient? HttpClient
    {
        get => _HttpClient ?? throw new InvalidOperationException($"Please call {nameof(InitializeAuthorization)} before any other method.");
        set => _HttpClient = value;
    }

    public void InitializeAuthorization(string login, string secretKey)
    {
        _HttpClient = new HttpClient() { BaseAddress = new Uri(K_QONTO_API_BASE_ADDRESS) };
        _ = _HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"{login}:{secretKey}");
    }
    public async Task<List<Transaction>> GetBankStatementAsync(string iban, DateTime from, DateTime to)
    {
        if (to == to.Date)
            to = to.AddDays(1).AddSeconds(-1);

        var allTransactions = new List<Transaction>();
        var currentPage = 1;
        bool hasNextPage;
        do
        {
#pragma warning disable CS8602 // Incorrect nullable flow analysis for indirect null check.
            var stream = await HttpClient.GetStreamAsync($"/v2/transactions?sort_by=settled_at:asc&iban={iban}" +
                $"&current_page={currentPage}" +
                $"&settled_at_from={from:o}" +
                $"&settled_at_to={to:o}"); // 2021-03-03T16:06:38.000Z
#pragma warning restore CS8602 // Incorrect nullable flow analysis for indirect null check.

            var transactions = await JsonSerializer.DeserializeAsync<TransactionsQuery>(stream);

            if (transactions == null)
                break;

            allTransactions.AddRange(transactions?.Transactions ?? Enumerable.Empty<Transaction>());
            currentPage++;
            hasNextPage = transactions?.Meta?.NextPage != null;
        } while (hasNextPage);

        return allTransactions;
    }
    public async Task<(string? LegalName, BankAccount? BankAccount)> GetCompanyInfo(string iban)
    {
#pragma warning disable CS8602 // Incorrect nullable flow analysis for indirect null check.
        var stream = await HttpClient.GetStreamAsync("/v2/organization");
#pragma warning restore CS8602 // Incorrect nullable flow analysis for indirect null check.

        var organization = await JsonSerializer.DeserializeAsync<OrganizationQuery>(stream);
        var account = organization?.Organization?.BankAccounts?.FirstOrDefault(a => a.IBAN?.ToLower() == iban.ToLower());
        
        return account == null ? 
            (null, null) : 
            (organization?.Organization?.LegalName, account);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _HttpClient?.Dispose();
            }
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}