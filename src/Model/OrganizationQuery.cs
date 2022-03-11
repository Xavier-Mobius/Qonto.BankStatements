using System.Text.Json.Serialization;

namespace Mobius.Qonto.BankStatements.Model;


public class OrganizationQuery
{
    [JsonPropertyName("organization")]
    public Organization? Organization { get; set; }
}
