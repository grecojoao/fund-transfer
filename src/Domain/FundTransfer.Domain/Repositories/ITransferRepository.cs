using FundTransfer.Domain.Dtos;

namespace FundTransfer.Domain.Repositories
{
    public interface ITransferRepository
    {
        Task AddAsync(Transfer transferDto, CancellationToken cancellationToken);
        Task<Transfer> GetAsync(Guid transactionId, CancellationToken cancellationToken);
    }
}