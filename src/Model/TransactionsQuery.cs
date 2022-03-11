using System.Text.Json.Serialization;

namespace Mobius.Qonto.BankStatements.Model;

public class TransactionsQuery
{
    [JsonPropertyName("transactions")]
    public Transaction[]? Transactions { get; set; }

    [JsonPropertyName("meta")]
    public Meta? Meta { get; set; }
}
