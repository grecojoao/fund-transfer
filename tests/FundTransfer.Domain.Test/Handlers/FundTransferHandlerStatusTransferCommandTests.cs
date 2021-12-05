using FlyGon.Notifications;
using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Entities;
using FundTransfer.Domain.Entities.Enums;
using FundTransfer.Domain.Handlers;
using FundTransfer.Domain.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundTransfer.Domain.Test.Handlers
{
    partial class FundTransferHandlerStatusTransferCommandTests
    {
        private FundTransferHandler _handler;

        [SetUp]
        public void Setup()
        {
            var transferRepository = new Mock<ITransferRepository>();
            _handler = new FundTransferHandler(transferRepository.Object);
        }

        [Test]
        [TestCase(null)]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        [Category("/Handlers/FundTransferHandler/StatusTransferCommand")]
        public async Task ValidatingWrongCommandsStatusTransferCommand(Guid? transactionId)
        {
            var wrong = new StatusTransferCommand(transactionId);
            var commandResult = await _handler.Handle(wrong, default);
            Assert.False(commandResult.Sucess);
            Assert.True(((IReadOnlyCollection<Notification>)commandResult.Data).Count == 1);
        }

        [Test]
        [TestCase("a4d4edbb-f9a9-4a59-9398-ed24ecb0dbfc")]
        [Category("/Handlers/FundTransferHandler/StatusTransferCommand")]
        public async Task ValidatingRightCommandsStatusTransferCommandWithErrorStatus(Guid? transactionId)
        {
            var transferRepository = new Mock<ITransferRepository>();
            transferRepository
                .Setup(x => x.GetAsync((Guid)transactionId, default))
                .Returns(Task.FromResult((Transfer)null));

            _handler = new FundTransferHandler(transferRepository.Object);
            var right = new StatusTransferCommand(transactionId);
            var commandResult = await _handler.Handle(right, default);
            Assert.False(commandResult.Sucess);
            Assert.True(commandResult.Message == "Invalid transaction number");
            Assert.True((TransferStatusEnum)commandResult.Data == TransferStatusEnum.Error);
        }

        [Test]
        [TestCase("a4d4edbb-f9a9-4a59-9398-ed24ecb0dbfc")]
        [Category("/Handlers/FundTransferHandler/StatusTransferCommand")]
        public async Task ValidatingRightCommandsStatusTransferCommandWithInQueueStatus(Guid? transactionId)
        {
            var transferRepository = new Mock<ITransferRepository>();
            transferRepository
                .Setup(x => x.GetAsync((Guid)transactionId, default))
                .Returns(Task.FromResult(
                    new Transfer((Guid)transactionId, TransferStatusEnum.InQueue, "123", "321", 0.01f)));

            _handler = new FundTransferHandler(transferRepository.Object);
            var right = new StatusTransferCommand(transactionId);
            var commandResult = await _handler.Handle(right, default);
            Assert.True(commandResult.Sucess);
            Assert.True(commandResult.Message == "");
            Assert.True((TransferStatusEnum)commandResult.Data == TransferStatusEnum.InQueue);
        }

        [Test]
        [TestCase("a4d4edbb-f9a9-4a59-9398-ed24ecb0dbfc")]
        [Category("/Handlers/FundTransferHandler/StatusTransferCommand")]
        public async Task ValidatingRightCommandsStatusTransferCommandWithException(Guid? transactionId)
        {
            var message = "Ocorreu um erro ao consultar no banco de dados";
            var transferRepository = new Mock<ITransferRepository>();
            transferRepository
                .Setup(x => x.GetAsync((Guid)transactionId, default))
                .Throws(new Exception(message));

            _handler = new FundTransferHandler(transferRepository.Object);
            var right = new StatusTransferCommand(transactionId);
            var commandResult = await _handler.Handle(right, default);
            Assert.False(commandResult.Sucess);
            Assert.True(commandResult.Message == message);
        }
    }
}