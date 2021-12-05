using FundTransfer.Domain.Entities;

namespace FundTransfer.Domain.Repositories
{
    public interface ITransferRepository : ITransaction
    {
        Task AddAsync(Transfer transfer, CancellationToken cancellationToken);
        Task UpdateStatus(Transfer transfer);
        Task<Transfer> GetAsync(Guid transactionId, CancellationToken cancellationToken);
    }
}