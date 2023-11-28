using System;
using System.Text.RegularExpressions;

namespace FamilyCalendarDotNet.Utilities
{
	public class EmailValidator
	{
        public static bool IsEmailValid(string email)
        {
            // Validate the email using regex
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
	}
}

