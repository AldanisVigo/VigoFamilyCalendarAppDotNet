using System.ComponentModel.DataAnnotations;

namespace FamilyCalendarDotNet.Models
{
	public class RecoveryChangePasswordDTO : IValidatableObject
    {
		public required string Code { get; set; }
		public required string NewPassword { get; set; }
		public required string NewPasswordConfirmation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check that the code is not empty
            if(Code.Length == 0)
            {
                yield return new ValidationResult("This recovery link is invalid.");
            }

            // Check that the passwords are provided, at least 8 characters long and matching
            if (NewPassword.Length == 0 || NewPasswordConfirmation.Length == 0)
            {
                yield return new ValidationResult("You must include a password and password confirmation.");
            }
            else if (NewPassword.Length < 8 || NewPasswordConfirmation.Length < 8)
            {
                yield return new ValidationResult("Your password must be at least 8 characters long.");
            }
            else if (NewPassword != NewPasswordConfirmation)
            {
                yield return new ValidationResult("The password and password confirmation provided do not match. Please check your passwords.");
            }
        }
    }
}

