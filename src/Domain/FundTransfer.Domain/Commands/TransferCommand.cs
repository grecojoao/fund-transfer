using FlyGon.CQRS.Commands;
using FlyGon.Notifications.Validations;

namespace FundTransfer.Domain.Commands
{
    public class TransferCommand : Command
    {
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
                .IsNotNullOrEmpty(AccountOrigin, nameof(AccountOrigin), "A conta de origem deve ser informada")
                .IsNotNullOrEmpty(AccountDestination, nameof(AccountDestination), "A conta destino deve ser informada")
                .IsNotNull(Value, nameof(Value), "O valor da transferência deve ser informado"));
            if (Value != null)
                AddNotifications(new ValidationContract()
                    .IsGreaterThan((float)Value, 0, nameof(Value), "O valor da transferência deve ser maior do que R$ 0,00"));
        }
    }
}