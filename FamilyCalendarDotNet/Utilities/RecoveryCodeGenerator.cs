namespace FamilyCalendarDotNet.Utilities
{
	public class RecoveryCodeGenerator
	{
        public static string GenerateCode()
        {
            string UUID = Guid.NewGuid().ToString();
            return UUID;
        }
	}
}

