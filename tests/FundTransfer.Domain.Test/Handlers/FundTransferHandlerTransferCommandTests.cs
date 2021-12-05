using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Handlers;
using FundTransfer.Domain.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FundTransfer.Domain.Test.Handlers
{
    partial class FundTransferHandlerTransferCommandTests
    {
        private FundTransferHandler _handler;

        [SetUp]
        public void Setup()
        {
            var transferRepository = new Mock<ITransferRepository>();
            _handler = new FundTransferHandler(transferRepository.Object);
        }

        [Test]
        [TestCase("", "", 0f)]
        [TestCase("", "", -0.01f)]
        [TestCase(null, null, null)]
        [Category("/Handlers/FundTransferHandler/TransferCommand")]
        public async Task ValidatingWrongCommandsTransferCommand(
            string accountOrigin, string accountDestination, float? value)
        {
            var wrong = new TransferCommand(accountOrigin, accountDestination, value);
            var commandResult = await _handler.Handle(wrong, default);
            Assert.True(commandResult.Sucess);
            Assert.True(commandResult.Message == "Error");
        }

        [Test]
        [TestCase("123", "321", 0.01f)]
        [Category("/Handlers/FundTransferHandler/TransferCommand")]
        public async Task ValidatingRightCommandsTransferCommand(
            string accountOrigin, string accountDestination, float? value)
        {
            var wrong = new TransferCommand(accountOrigin, accountDestination, value);
            var commandResult = await _handler.Handle(wrong, default);
            Assert.True(commandResult.Sucess);
            Assert.True(commandResult.Message == "InQueue");
        }

        [Test]
        [TestCase("123", "321", 0.01f)]
        [Category("/Handlers/FundTransferHandler/TransferCommand")]
        public async Task ValidatingRightCommandsTransferCommandWithException(
            string accountOrigin, string accountDestination, float? value)
        {
            var message = "Ocorreu um erro ao criar a transação";
            var transferRepository = new Mock<ITransferRepository>();
            transferRepository
                .Setup(x => x.BeginTransactionAsync(default))
                .Throws(new Exception(message));

            _handler = new FundTransferHandler(transferRepository.Object);
            var right = new TransferCommand(accountOrigin, accountDestination, value);
            var commandResult = await _handler.Handle(right, default);
            Assert.False(commandResult.Sucess);
            Assert.True(commandResult.Message == message);
        }
    }
}