using FlyGon.CQRS.Commands;
using FlyGon.Notifications.Validations;

namespace FundTransfer.Domain.Commands
{
    public class StatusTransferCommand : Command
    {
        public Guid? TransactionId { get; private set; }

        public StatusTransferCommand(Guid? transactionId) =>
            TransactionId = transactionId;

        public override void Validate()
        {
            AddNotifications(new ValidationContract()
                .IsNotNull(TransactionId, nameof(TransactionId), "O identificador da transação deve ser informado"));
            if (TransactionId != null)
                AddNotifications(new ValidationContract()
                    .IsNotEmpty((Guid)TransactionId, nameof(TransactionId), "O identificador da transação deve ser informado"));
        }
    }
}