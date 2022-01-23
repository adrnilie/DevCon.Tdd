using System;

namespace DevCon.Code.Business.Dto
{
    public class DevConUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}