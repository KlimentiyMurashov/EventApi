namespace Application.DTOs
{
	public class EventDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime DateTime { get; set; }
		public string Location { get; set; }
		public string Category { get; set; }
		public int MaxParticipants { get; set; }
		public string? ImageUrl { get; set; }
		public List<EventRegistrationDto> EventRegistrations { get; set; } = new List<EventRegistrationDto>();
	}
}
