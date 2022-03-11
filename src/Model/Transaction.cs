using System.Text.Json.Serialization;

namespace Mobius.Qonto.BankStatements.Model;

public class Transaction
{
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("settled_at")]
    public DateTime SettledAt { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("local_amount")]
    public decimal LocalAmount { get; set; }

    [JsonPropertyName("side")]
    public string? Side { get; set; }

    [JsonPropertyName("operation_type")]
    public string? OperationType { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("local_currency")]
    public string? LocalCurrency { get; set; }

    public override string ToString() => $"TransactionId: {TransactionId} | "
        + $"Amount: {Amount} | "
        + $"Side: {Side} | "
        + $"OperationType: {OperationType} | "
        + $"Currency: {Currency} | "
        + $"Label: {Label} | "
        + $"SettledAt: {SettledAt} | "
        + $"Reference: {Reference}";
}
