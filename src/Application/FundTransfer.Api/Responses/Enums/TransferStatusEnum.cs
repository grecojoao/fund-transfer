using System.Text.Json.Serialization;

namespace FundTransfer.Api.Responses.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransferStatusEnum
    {
        InQueue,
        Processing,
        Confirmed,
        Error
    }
}