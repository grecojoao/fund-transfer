using FundTransfer.Domain.Dtos;
using FundTransfer.Domain.Repositories;
using Raven.Client.Documents;

namespace FundTransfer.Infra.Storage.RavenDb.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly DocumentStore _documentStore;

        public TransferRepository(DocumentStore documentStore)
        {
            _documentStore = documentStore;
            _documentStore.Initialize();
        }

        public Task AddAsync(Transfer transferDto, CancellationToken cancellationToken)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(transferDto, transferDto.TransactionId.ToString());
                session.SaveChanges();
                return Task.CompletedTask;
            };
        }

        public Task<Transfer> GetAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            using (var session = _documentStore.OpenSession())
            {
                var transfer = session.Load<Transfer>(transactionId.ToString());
                return Task.FromResult(transfer);
            };
        }
    }
}