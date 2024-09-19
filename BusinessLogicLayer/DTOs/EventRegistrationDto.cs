using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
	public class EventRegistrationDto
	{
		public int ParticipantId { get; set; }
		public int EventId { get; set; }
		public DateTime RegistrationDate { get; set; }
		public string ParticipantName { get; set; }
		public string EventTitle { get; set; }
	}
}
