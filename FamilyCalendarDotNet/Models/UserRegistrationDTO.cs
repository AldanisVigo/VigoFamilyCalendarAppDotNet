using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FamilyCalendarDotNet.Utilities;

namespace FamilyCalendarDotNet.Models
{
	public class UserRegistrationDTO : IValidatableObject
	{
        public required string FullName { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirmation { get; set; }
        public required DateTime RegistrationDate { get; set; }
        public required AccountType? UserAccountType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Make sure a full name was provided.
            if (FullName.Length == 0)
            {
                yield return new ValidationResult("Full name is missing");
            }

            // Make sure the email was provided
            if (Email.Length == 0)
            {
                yield return new ValidationResult("Email address is missing");
            }
            else
            {
                // Validate the email using regex
                if(!EmailValidator.IsEmailValid(Email))
                {
                    yield return new ValidationResult("Email address is invalid");
                }
            }

            // Make sure a username was provided
            if (Username.Length == 0)
            {
                yield return new ValidationResult("Username is missing");
            }else if(Username.Length > 30)
            {
                yield return new ValidationResult("The username provided is greater than 30 characters long. Please limit the username to 30 characters and try again");
            }else if(Username.Length < 8)
            {
                yield return new ValidationResult("The username provided is less than 8 characters long. Please make the username at least 8 characters long and try again");
            }

            // Check that there are no special characters in the username.
            char[] specialChars = { '~','`','!','@','#','$','%','^','&','*','(',')','_','-','+','=','<','>',',','.',':',';','\'','"','/','\\','|','[','{',']','}',' ','\n','\r','\t'};
            bool hasSpecialChars = false;
            foreach(char c in specialChars)
            {
                if(Username.IndexOf(c) > -1)
                {
                    hasSpecialChars = true;
                    break;
                }
            }
            if (hasSpecialChars)
            {
                yield return new ValidationResult("The username provided contains special characters. Please remove all special characters from the username and try again");
            }

            // Check that the passwords are provided, at least 8 characters long and matching
            if (Password.Length == 0 || PasswordConfirmation.Length == 0)
            {
                yield return new ValidationResult("You must include a password and password confirmation");
            } else if(Password.Length < 8 || PasswordConfirmation.Length < 8) 
            {
                yield return new ValidationResult("Your password must be at least 8 characters long");
            }
            else if (Password != PasswordConfirmation)
            {
                yield return new ValidationResult("The password and password confirmation provided do not match. Please check your passwords");
            }
        }
    }
}

