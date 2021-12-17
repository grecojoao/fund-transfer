using FlyGon.CQRS.Commands;
using FlyGon.Notifications.Validations;

namespace FundTransfer.Domain.Commands
{
    public class StatusTransferCommand : Command
    {
        private const string INVALID_TRANSACTION_NUMBER = "The transaction identifier must be informed";

        public Guid? TransactionId { get; private set; }

        public StatusTransferCommand(Guid? transactionId) =>
            TransactionId = transactionId;

        public override void Validate()
        {
            AddNotifications(new ValidationContract()
                .IsNotNull(TransactionId, nameof(TransactionId), INVALID_TRANSACTION_NUMBER));
            if (TransactionId != null)
                AddNotifications(new ValidationContract()
                    .IsNotEmpty((Guid)TransactionId, nameof(TransactionId), INVALID_TRANSACTION_NUMBER));
        }
    }
}