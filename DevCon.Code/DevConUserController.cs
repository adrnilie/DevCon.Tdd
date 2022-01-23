using System;
using System.Net;
using System.Threading.Tasks;
using DevCon.Code.Business.Services;
using DevCon.Code.Common;
using DevCon.Code.Mappings;
using DevCon.Code.Messaging.Handlers;
using DevCon.Code.Messaging.Helpers;
using DevCon.Code.Models.Request;
using DevCon.Code.Models.Response;
using DevCon.Code.Validation;
using Microsoft.AspNetCore.Mvc;

namespace DevCon.Code
{
    public class DevConUserController
    {
        private readonly IDevConUserService _devConUserService;
        private readonly ITokenProviderService _tokenProvider;
        private readonly ICommandsHandler _commandsHandler;

        public DevConUserController(IDevConUserService devConUserService, ITokenProviderService tokenProvider, ICommandsHandler commandsHandler)
        {
            _devConUserService = devConUserService;
            _tokenProvider = tokenProvider;
            _commandsHandler = commandsHandler;
        }

        public async Task<ActionResult<Response<DevConUserResponse>>> CreateDevConUser(DevConNewUserRequest request)
        {
            var validationResult = request.Validate();
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(Response.WithError(HttpStatusCode.BadRequest, validationResult.ErrorContext));
            }

            var response = await _devConUserService.CreateUserAsync(request.AdaptToModel()).ConfigureAwait(false);
            if (!response.Success)
            {
                return GetObjectResultByServiceResponse(response);
            }

            await SendUserConfirmEmail(response.Result.Id).ConfigureAwait(false);

            await SendUserCreatedToDataWarehouse(response.Result.Name, response.Result.EmailAddress).ConfigureAwait(false);

            return new OkObjectResult(Response<DevConUserResponse>.WithResult(response.Result.AdaptToResponse()));
        }

        private static ActionResult GetObjectResultByServiceResponse(ServiceResponse serviceResponse)
        {
            return serviceResponse.StatusCode switch
            {
                (int)HttpStatusCode.BadRequest => new BadRequestObjectResult(
                    Response.WithError(HttpStatusCode.BadRequest, serviceResponse.ErrorMessage)),
                (int)HttpStatusCode.NotFound => new NotFoundObjectResult(Response.WithError(HttpStatusCode.NotFound,
                    serviceResponse.ErrorMessage)),
                (int)HttpStatusCode.Found => new ConflictObjectResult(Response.WithError(HttpStatusCode.Conflict,
                    serviceResponse.ErrorMessage)),
                (int)HttpStatusCode.InternalServerError => new ConflictObjectResult(
                    Response.WithError(HttpStatusCode.InternalServerError, serviceResponse.ErrorMessage)),
                _ => new BadRequestObjectResult(
                    Response.WithError(HttpStatusCode.BadRequest, "Something is not right."))
            };
        }

        private async Task SendUserConfirmEmail(Guid userId)
        {
            var confirmEmailToken = await _tokenProvider.GenerateConfirmEmailTokenAsync().ConfigureAwait(false);
            var command = MessagingHelper.GetSendUserConfirmEmailCommand(userId, confirmEmailToken);
            await _commandsHandler.SendUserConfirmEmailCommandAsync(command).ConfigureAwait(false);
        }

        private async Task SendUserCreatedToDataWarehouse(string name, string emailAddress)
        {
            var command = MessagingHelper.GeUserCreatedToDataWarehouseCommand(name, emailAddress);
            await _commandsHandler.SendUserCreatedToDataWarehouseCommandAsync(command).ConfigureAwait(false);
        }
    }
}
