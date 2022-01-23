using System;
using System.Text;
using System.Text.RegularExpressions;
using DevCon.Code.Common;
using DevCon.Code.Models.Request;

namespace DevCon.Code.Validation
{
    public class ValidatorContext
    {
        public string ErrorContext { get; set; }
        public bool IsValid => string.IsNullOrWhiteSpace(ErrorContext);
    }

    public static class Validator
    {
        public static ValidatorContext Validate(this DevConNewUserRequest request)
        {
            var errorContextBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                errorContextBuilder.Append(ErrorMessages.Validation.EmailAddressRequired);
            } else if (!EmailMatch(request.EmailAddress).Success)
            {
                errorContextBuilder.Append(ErrorMessages.Validation.InvalidEmailAddress);
            }

            errorContextBuilder.Append(ValidatePasswordAndConfirmPassword(request));

            return new ValidatorContext
            {
                ErrorContext = errorContextBuilder.ToString()
            };
        }

        private static Match EmailMatch(string emailAddress)
        {
            const string regexPattern = @"^\S+@\S+$";
            return Regex.Match(emailAddress, regexPattern, RegexOptions.IgnoreCase);
        }

        private static string ValidatePasswordAndConfirmPassword(DevConNewUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ErrorMessages.Validation.PasswordRequired;
            }

            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return ErrorMessages.Validation.ConfirmPasswordRequired;
            }

            if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.InvariantCulture))
            {
                return ErrorMessages.Validation.PasswordDontMatch;
            }

            return string.Empty;
        }
    }
}