using FlyGon.CQRS.Commands;
using FlyGon.CQRS.Handlers.Contracts;
using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Entities;
using FundTransfer.Domain.Entities.Enums;
using FundTransfer.Domain.Repositories;

namespace FundTransfer.Domain.Handlers
{
    public class FundTransferHandler :
        ICommandHandler<TransferCommand, CommandResult>,
        ICommandHandler<StatusTransferCommand, CommandResult>
    {
        private readonly ITransferRepository _transferRepository;

        public FundTransferHandler(ITransferRepository transferRepository) =>
            _transferRepository = transferRepository;

        public async Task<CommandResult> Handle(TransferCommand command, CancellationToken cancellationToken)
        {
            command.Validate();
            var transfer = command.IsValid ?
                new Transfer(
                    Guid.NewGuid(), TransferStatusEnum.InQueue, command.AccountOrigin, command.AccountDestination,
                    command.Value) :
                new Transfer(
                    Guid.NewGuid(), TransferStatusEnum.Error, command.AccountOrigin, command.AccountDestination,
                    command.Value, command.NotificationsMessage());
            try
            {
                await _transferRepository.BeginTransactionAsync(cancellationToken);
                await _transferRepository.AddAsync(transfer, cancellationToken);
                await _transferRepository.SaveChangesAsync(cancellationToken);
                await _transferRepository.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _transferRepository.RollBackAsync(cancellationToken);
                return new CommandResult(false, ex?.Message);
            }
            return new CommandResult(true, transfer.TransferStatusEnum.ToString(), transfer.TransactionId);
        }

        public async Task<CommandResult> Handle(StatusTransferCommand command, CancellationToken cancellationToken)
        {
            command.Validate();
            if (command.IsInvalid)
                return new CommandResult(false, command.NotificationsMessage(), command.Notifications);

            Transfer transfer;
            try
            {
                transfer = await _transferRepository.GetAsync((Guid)command.TransactionId, cancellationToken);
            }
            catch (Exception ex)
            {
                return new CommandResult(false, ex?.Message);
            }

            if (transfer == null)
                return new CommandResult(false, "Invalid transaction number", TransferStatusEnum.Error);
            return new CommandResult(true, transfer.Message, transfer.TransferStatusEnum);
        }
    }
}