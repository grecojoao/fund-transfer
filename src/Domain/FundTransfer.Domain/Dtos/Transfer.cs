using FundTransfer.Domain.Enums;

namespace FundTransfer.Domain.Dtos
{
    public class Transfer
    {
        public Guid TransactionId { get; set; }
        public TransferStatusEnum TransferStatus { get; set; }
        public string Message { get; set; }

        public Transfer() { }

        public Transfer(Guid transactionId, TransferStatusEnum transferStatus, string message = "")
        {
            TransactionId = transactionId;
            TransferStatus = transferStatus;
            Message = message;
        }
    }
}