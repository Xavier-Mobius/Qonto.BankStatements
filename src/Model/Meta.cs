using System.Text.Json.Serialization;

namespace Mobius.Qonto.BankStatements.Model;
public class Meta
{
    [JsonPropertyName("next_page")]
    public int? NextPage { get; set; }
}
