using FundTransfer.Api.Responses.Enums;

namespace FundTransfer.Api.Responses
{
    public record StatusTransferResponse(TransferStatusEnum TransferStatusEnum, string Message = "") { }
}