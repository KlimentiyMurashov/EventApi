using Application.DTOs;
using Domain.Entities;
using AutoMapper;

namespace Application.MappingProfile
{
	public class MappingParticipantProfile : Profile
	{
		public MappingParticipantProfile()
		{
			CreateMap<Participant, ParticipantDto>()
				.ForMember(dest => dest.EventRegistrations, opt => opt.MapFrom(src => src.EventRegistrations
					.Select(er => new EventRegistrationDto
					{
						ParticipantId = er.ParticipantId, 
						EventId = er.EventId,
						RegistrationDate = er.RegistrationDate
					})));

			CreateMap<ParticipantDto, Participant>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.EventRegistrations, opt => opt.Ignore());
		}
	}
}
