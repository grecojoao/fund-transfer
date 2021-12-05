using FlyGon.Notifications;
using FundTransfer.Api.Requests;
using FundTransfer.Api.Responses;
using FundTransfer.Api.Responses.Enums;
using FundTransfer.Domain.Commands;
using FundTransfer.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace FundTransfer.Api.Controllers
{
    [ApiController]
    [Route("api/fund-transfer")]
    public class FundTransferController : ControllerBase
    {
        private readonly FundTransferHandler _handler;

        public FundTransferController(FundTransferHandler handler) =>
            _handler = handler;

        [HttpPost()]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransferResponse>> Transfer(
            [FromBody] TransferRequest transfer,
            CancellationToken cancellationToken = default)
        {
            var command = new TransferCommand(transfer.AccountOrigin, transfer.AccountDestination, transfer.Value);
            var commandResult = await _handler.Handle(command, cancellationToken);
            return commandResult.Sucess ?
                Accepted(new TransferResponse((Guid)commandResult.Data)) :
                StatusCode(StatusCodes.Status500InternalServerError, new Error(commandResult.Message));
        }

        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(StatusTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusTransferResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(StatusTransferResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StatusTransferResponse>> StatusTransfer(
            Guid? transactionId,
            CancellationToken cancellationToken = default)
        {
            var command = new StatusTransferCommand(transactionId);
            var commandResult = await _handler.Handle(command, cancellationToken);

            if (!commandResult.Sucess && commandResult.Data == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Error(commandResult.Message));
            else if (!commandResult.Sucess && commandResult.Data.GetType() == typeof(Notification[]))
                return BadRequest(new StatusTransferResponse(TransferStatusEnum.Error, commandResult.Message));

            var response = new StatusTransferResponse((TransferStatusEnum)commandResult.Data, commandResult.Message);
            return commandResult.Sucess ?
                Ok(response) :
                NotFound(response);
        }
    }
}