using System.Text.Json.Serialization;

namespace DataAccessLayer.Entities
{
	public class Participant
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Email { get; set; }

		[JsonIgnore]
		public List<EventRegistration> EventRegistrations { get; set; } = new();
	}
}
