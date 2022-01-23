using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevCon.Code.Messaging.Repository
{
    public interface IMessagingRepository
    {
        List<SentMessage> SentMessages { get; }
        Task SendUserConfirmEmailCommandAsync(SendUserConfirmEmailCommand command);
        Task SendUserCreatedToDataWarehouseCommandAsync(SendUserCreatedToDataWarehouseCommand command);
    }

    internal class MessagingRepository : IMessagingRepository
    {
        public MessagingRepository()
        {
            SentMessages = new List<SentMessage>();
        }

        public List<SentMessage> SentMessages { get; }

        public Task SendUserConfirmEmailCommandAsync(SendUserConfirmEmailCommand command)
        {
            SendMessage(command);
            return Task.CompletedTask;
        }

        public Task SendUserCreatedToDataWarehouseCommandAsync(SendUserCreatedToDataWarehouseCommand command)
        {
            SendMessage(command);
            return Task.CompletedTask;
        }

        private void SendMessage(object command)
        {
            SentMessages.Add(new SentMessage
            {
                Message = command
            });
        }
    }
    
    public class SentMessage
    {
        public object Message { get; set; }
    }
}