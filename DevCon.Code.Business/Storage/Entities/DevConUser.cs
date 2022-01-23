using System;
using System.ComponentModel.DataAnnotations;

namespace DevCon.Code.Business.Storage.Entities
{
    public class DevConUser
    {
        public DevConUser()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
            LastUpdated = DateTime.UtcNow;
            Active = false;
            EmailConfirmed = false;
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Active { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}