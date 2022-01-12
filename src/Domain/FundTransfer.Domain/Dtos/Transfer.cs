using FundTransfer.Domain.Enums;

namespace FundTransfer.Domain.Dtos
{
    public class Transfer
    {
        public Guid TransactionId { get; private set; }
        public TransferStatusEnum TransferStatus { get; private set; }
        public string Message { get; private set; }
        public string AccountOrigin { get; private set; }
        public string AccountDestination { get; private set; }
        public float? Value { get; private set; }

        public Transfer() { }

        public Transfer(
            Guid transactionId, TransferStatusEnum transferStatus, string accountOrigin, string accountDestination, float? value, string message = "")
        {
            TransactionId = transactionId;
            TransferStatus = transferStatus;
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
            Message = message;
        }

        public void Change(TransferStatusEnum transferStatus, string message = "")
        {
            TransferStatus = transferStatus;
            Message = message;
        }
    }
}