namespace DevCon.Code.Common
{
    public static class ErrorMessages
    {
        public const string UserNotFound = "No user found for the specified identifier";
        public const string UserAlreadyExists = "The user already exists";
        public const string CreateUserExecutionFailed = "The creation of the user has failed.";
        public const string NewUserDetailsNotProvided = "We couldn't find any details about your new user";

        public static class Validation
        {
            public const string EmailAddressRequired = "EmailAddress is required.\n";
            public const string InvalidEmailAddress = "EmailAddress format is not required\n";
            public const string PasswordRequired = "Password is required.\n";
            public const string ConfirmPasswordRequired = "ConfirmPassword is required.\n";
            public const string PasswordDontMatch = "Passwords do not match.\n";
        }
    }
}