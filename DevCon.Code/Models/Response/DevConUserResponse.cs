using System;

namespace DevCon.Code.Models.Response
{
    public class DevConUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}