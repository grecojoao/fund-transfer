using FundTransfer.Domain.Entities.Enums;

namespace FundTransfer.Domain.Entities
{
    public class Transfer
    {
        public long Id { get; set; }
        public Guid TransactionId { get; private set; }
        public TransferStatusEnum TransferStatusEnum { get; private set; }
        public string Message { get; private set; }
        public string AccountOrigin { get; private set; }
        public string AccountDestination { get; private set; }
        public float? Value { get; private set; }

        public Transfer() { }

        public Transfer(
            Guid transactionId, TransferStatusEnum transferStatusEnum, string accountOrigin,
            string accountDestination, float? value, string message = "")
        {
            TransactionId = transactionId;
            TransferStatusEnum = transferStatusEnum;
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
            Message = message;
        }

        public void Change(TransferStatusEnum transferStatusEnum) =>
            TransferStatusEnum = transferStatusEnum;

        public void Change(TransferStatusEnum transferStatusEnum, string message)
        {
            TransferStatusEnum = transferStatusEnum;
            Message = message;
        }
    }
}