using FundTransfer.Api.Responses.Enums;

namespace FundTransfer.Api.Responses
{
    public record StatusTransferResponse(TransferStatusEnum TransferStatus, string Message = "") { }
}