namespace DevCon.Code.Models.Request
{
    public class DevConNewUserRequest
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}