/*
 * This is the model of a single user data transfer object.
 * Used for representing the structure of the body of our HTTP requests.
 */
namespace FamilyCalendarDotNet.Models
{
	public class UserDTO
	{
        public required string Username { get; set; }
        public required string Password { get; set; }
	}
}

