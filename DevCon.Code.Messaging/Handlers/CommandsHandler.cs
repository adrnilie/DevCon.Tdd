using System.Threading.Tasks;
using DevCon.Code.Messaging.Repository;

namespace DevCon.Code.Messaging.Handlers
{
    public interface ICommandsHandler
    {
        Task SendUserConfirmEmailCommandAsync(SendUserConfirmEmailCommand command);
        Task SendUserCreatedToDataWarehouseCommandAsync(SendUserCreatedToDataWarehouseCommand command);
    }

    public class CommandsHandler : ICommandsHandler
    {
        private readonly IMessagingRepository _messagingRepository;

        public CommandsHandler(IMessagingRepository messagingRepository)
        {
            _messagingRepository = messagingRepository;
        }

        public async Task SendUserConfirmEmailCommandAsync(SendUserConfirmEmailCommand command)
        {
            await _messagingRepository.SendUserConfirmEmailCommandAsync(command).ConfigureAwait(false);
        }

        public async Task SendUserCreatedToDataWarehouseCommandAsync(SendUserCreatedToDataWarehouseCommand command)
        {
            await _messagingRepository.SendUserCreatedToDataWarehouseCommandAsync(command).ConfigureAwait(false);
        }
    }
}