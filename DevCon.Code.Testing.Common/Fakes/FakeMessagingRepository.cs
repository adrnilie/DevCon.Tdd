using System.Collections.Generic;
using System.Threading.Tasks;
using DevCon.Code.Messaging;
using DevCon.Code.Messaging.Repository;

namespace DevCon.Code.Testing.Common.Fakes
{
    public class FakeMessagingRepository : IMessagingRepository
    {
        public FakeMessagingRepository()
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
}