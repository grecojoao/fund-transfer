using FundTransfer.Domain.Commands;
using NUnit.Framework;
using System;

namespace FundTransfer.Domain.Test.Commands
{
    class StatusTransferCommandTests
    {
        [Test]
        [TestCase(null)]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        [Category("/Commands/StatusTransferCommand")]
        public void ValidatingWrongCommand(Guid? transactionId)
        {
            var wrong = new StatusTransferCommand(transactionId);
            wrong.Validate();
            Assert.True(wrong.IsInvalid);
            Assert.True(wrong.Notifications.Count == 1);
        }

        [Test]
        [TestCase("a4d4edbb-f9a9-4a59-9398-ed24ecb0dbfc")]
        [Category("/Commands/StatusTransferCommand")]
        public void ValidatingRightCommand(Guid? transactionId)
        {
            var right = new StatusTransferCommand(transactionId);
            right.Validate();
            Assert.True(right.IsValid);
            Assert.True(right.Notifications.Count == 0);
        }
    }
}