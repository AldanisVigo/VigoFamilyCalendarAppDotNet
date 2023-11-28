/*
 * This is the model for a single calendar event item
 */
namespace FamilyCalendarDotNet
{
	public class CalendarEvent
	{
        public DateTime DateTime { get; set; }

		public string EventContent { get; set; }

        public CalendarEvent(DateTime dateTime, string eventContent) {
            DateTime = dateTime;
            EventContent = eventContent;
		}
	}
}

