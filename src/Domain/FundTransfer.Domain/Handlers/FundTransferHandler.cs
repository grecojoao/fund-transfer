using FlyGon.CQRS.Commands;
using FlyGon.CQRS.Handlers.Contracts;
using FundTransfer.Domain.Bus.Publishers;
using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Dtos;
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
            var transferDto = command.IsValid ?
                new Transfer(
                    Guid.NewGuid(), TransferStatusEnum.InQueue, command.AccountOrigin, command.AccountDestination,
                    command.Value) :
                new Transfer(
                    Guid.NewGuid(), TransferStatusEnum.Error, command.AccountOrigin, command.AccountDestination,
                    command.Value, command.NotificationsMessage());
            Log.Debug($"{JsonConvert.SerializeObject(transferDto)}");
            try
            {
                var transfer = new Entities.Transfer(transferDto.TransactionId, transferDto.TransferStatus, transferDto.Message);
                await _transferRepository.AddAsync(transfer, cancellationToken);
                if (transferDto.TransferStatus == TransferStatusEnum.InQueue)
                    await _busPublisher.SendAsync(JsonConvert.SerializeObject(transferDto));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return new CommandResult(false, ex?.Message);
            }
            return new CommandResult(true, transferDto.TransferStatus.ToString(), transferDto.TransactionId);
        }

        public async Task<CommandResult> Handle(StatusTransferCommand command, CancellationToken cancellationToken)
        {
            command.Validate();
            if (command.IsInvalid)
                return new CommandResult(false, command.NotificationsMessage(), command.Notifications);

            Entities.Transfer transfer;
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