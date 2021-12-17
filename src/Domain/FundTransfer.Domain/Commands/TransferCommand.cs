using FlyGon.CQRS.Commands;
using FlyGon.Notifications.Validations;

namespace FundTransfer.Domain.Commands
{
    public class TransferCommand : Command
    {
        private const string INVALID_BALANCE = "Invalid balance";
        private const string INVALID_ACCOUNT_NUMBER = "Invalid account number";

        public string AccountOrigin { get; private set; }
        public string AccountDestination { get; private set; }
        public float? Value { get; private set; }

        public TransferCommand(string accountOrigin, string accountDestination, float? value)
        {
            AccountOrigin = accountOrigin;
            AccountDestination = accountDestination;
            Value = value;
        }

        public override void Validate()
        {
            AddNotifications(new ValidationContract()
                .IsNotNullOrEmpty(AccountOrigin, nameof(AccountOrigin), INVALID_ACCOUNT_NUMBER)
                .IsNotNullOrEmpty(AccountDestination, nameof(AccountDestination), INVALID_ACCOUNT_NUMBER)
                .IsNotNull(Value, nameof(Value), INVALID_BALANCE));
            if (Value != null)
                AddNotifications(new ValidationContract()
                    .IsGreaterThan((float)Value, 0, nameof(Value), INVALID_BALANCE));
        }
    }
}