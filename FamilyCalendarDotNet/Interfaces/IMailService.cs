using FamilyCalendarDotNet.Models;

namespace FamilyCalendarDotNet.Interfaces
{
	public interface IMailService
	{
        bool SendMail(MailData mailData);
    }
}

