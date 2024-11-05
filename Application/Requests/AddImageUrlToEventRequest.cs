namespace Application.Requests
{
	public class AddImageUrlToEventRequest
	{
		public int EventId { get; set; }
		public string ImageUrl { get; set; }
	}

}
