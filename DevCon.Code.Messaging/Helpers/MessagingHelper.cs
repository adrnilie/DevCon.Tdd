using System;

namespace DevCon.Code.Messaging.Helpers
{
    public class MessagingHelper
    {
        public static SendUserConfirmEmailCommand GetSendUserConfirmEmailCommand(Guid userId, string token)
        {
            return new SendUserConfirmEmailCommand
            {
                CallbackUrl = $"https://www.identity.test/{userId}?token={token}"
            };
        }

        public static SendUserCreatedToDataWarehouseCommand GeUserCreatedToDataWarehouseCommand(string name, string emailAddress)
        {
            return new SendUserCreatedToDataWarehouseCommand
            {
                Name = name,
                EmailAddress = emailAddress
            };
        }
    }
}