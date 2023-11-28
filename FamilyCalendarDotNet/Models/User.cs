/*
 *  This is the model for a single user 
 */

namespace FamilyCalendarDotNet
{
	public class User 
	{
        public int Id { get; set; }

        public DateTime RegistrationDateTime { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public User(DateTime registrationDate, string username, string password)
        {
            RegistrationDateTime = registrationDate;
            Username = username;
            Password = password;
        }
    }
}

