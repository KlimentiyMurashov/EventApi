using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer.MappingProfile
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Participant, ParticipantDto>()
			.ForMember(dest => dest.EventRegistrations, opt => opt.MapFrom(src => src.EventRegistrations
				.Select(er => new EventRegistrationDto
				{
					EventId = er.EventId,
					RegistrationDate = er.RegistrationDate
				})));

			CreateMap<Event, EventDto>()
				.ForMember(dest => dest.EventRegistrations, opt => opt.MapFrom(src => src.EventRegistrations
					.Select(er => new EventRegistrationDto
					{
						ParticipantId = er.ParticipantId,
						RegistrationDate = er.RegistrationDate
					})));

			CreateMap<EventDto, Event>()
			.ForMember(dest => dest.Id, opt => opt.Ignore()) 
			.ForMember(dest => dest.EventRegistrations, opt => opt.Ignore()); 

			CreateMap<ParticipantDto, Participant>()
				.ForMember(dest => dest.Id, opt => opt.Ignore()) 
				.ForMember(dest => dest.EventRegistrations, opt => opt.Ignore());
		}
	}
}
