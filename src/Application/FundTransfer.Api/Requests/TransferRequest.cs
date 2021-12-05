namespace FundTransfer.Api.Requests
{
    public record TransferRequest(string AccountOrigin, string AccountDestination, float? Value) { }
}