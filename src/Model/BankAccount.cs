using System.Text.Json.Serialization;

namespace Mobius.Qonto.BankStatements.Model;
public class BankAccount
{
    [JsonPropertyName("iban")]
    public string? IBAN { get; set; }

    [JsonPropertyName("bic")]
    public string? BIC { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
