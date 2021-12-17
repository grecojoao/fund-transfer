using FlyGon.CQRS.Commands;
using FlyGon.CQRS.Handlers.Contracts;
using FundTransfer.Domain.Bus.Publishers;
using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Entities;
using FundTransfer.Domain.Enums;
using FundTransfer.Domain.Repositories;
using Newtonsoft.Json;
using Serilog;

namespace FundTransfer.Domain.Handlers
{
    public sealed class FundTransferHandler :
        ICommandHandler<TransferCommand, CommandResult>,
        ICommandHandler<StatusTransferCommand, CommandResult>
    {
        private const string INVALID_TRANSACTION_NUMBER = "Invalid transaction number";
        private readonly ITransferRepository _transferRepository;
        private readonly IBusPublisher _busPublisher;

        public FundTransferHandler(ITransferRepository transferRepository, IBusPublisher busPublisher)
        {
            _transferRepository = transferRepository;
            _busPublisher = busPublisher;
        }

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
            Log.Debug($"{JsonConvert.SerializeObject(transfer)}");
            try
            {
                var transferDto = new Dtos.Transfer(transfer.TransactionId, transfer.TransferStatus);
                await _transferRepository.AddAsync(transferDto, cancellationToken);
                await _busPublisher.SendAsync(JsonConvert.SerializeObject(transfer));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new CommandResult(false, ex?.Message);
            }
            return new CommandResult(true, transfer.TransferStatus.ToString(), transfer.TransactionId);
        }

        public async Task<CommandResult> Handle(StatusTransferCommand command, CancellationToken cancellationToken)
        {
            command.Validate();
            if (command.IsInvalid)
                return new CommandResult(false, command.NotificationsMessage(), command.Notifications);

            Dtos.Transfer transfer;
            try
            {
                transfer = await _transferRepository.GetAsync((Guid)command.TransactionId, cancellationToken); 
                Log.Debug($"{JsonConvert.SerializeObject(transfer)}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new CommandResult(false, ex?.Message);
            }

            if (transfer == null)
                return new CommandResult(false, INVALID_TRANSACTION_NUMBER, TransferStatusEnum.Error);
            return new CommandResult(true, transfer.Message, transfer.TransferStatus);
        }
    }
}