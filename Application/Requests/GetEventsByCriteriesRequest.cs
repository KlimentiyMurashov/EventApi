namespace Application.Requests
{
	public class GetEventsByCriteriesRequest
	{
		public DateTime? Date { get; set; }
		public string? Location { get; set; }
		public string? Category { get; set; }
	}

}
