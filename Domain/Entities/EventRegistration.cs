
namespace Domain.Entities
{
	public class EventRegistration
	{
		public int ParticipantId { get; set; }
		public Participant Participant { get; set; }

		public int EventId { get; set; }
		public Event Event { get; set; }

		public DateTime RegistrationDate { get; set; }
	}
}
