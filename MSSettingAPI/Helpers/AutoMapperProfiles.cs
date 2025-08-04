using AutoMapper;
using RRScout.DTOs;
using RRScout.Entities;
using TeamNames = RRScout.Entities.TeamNames;

namespace RRScout.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PicklistOrder, PicklistOrderDTO>().ReverseMap()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.eventCode, opt => opt.MapFrom(src => src.eventCode))
                .ForMember(dest => dest.teamNumber, opt => opt.MapFrom(src => src.teamNumber))
                .ForMember(dest => dest.order, opt => opt.MapFrom(src => src.order));

            CreateMap<MatchData_2023, MatchDataDTO_2023>().ReverseMap();
            CreateMap<MatchData_2024, MatchDataDTO_2024>().ReverseMap();
            CreateMap<MatchData_2025, MatchDataDTO_2025>().ReverseMap();
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<MatchSchedule, MatchScheduleDTO>().ReverseMap();
            CreateMap<SuperScoutData_2025, SuperScoutDataDTO_2025>().ReverseMap();

            CreateMap<TeamNames, TeamInfo>()
                .ForMember(dest => dest.eventCode, opt => opt.MapFrom(src => src.eventCode))
                .ForMember(dest => dest.team_number, opt => opt.MapFrom(src => src.teamNumber))
                .ForMember(dest => dest.nickname, opt => opt.MapFrom(src => src.teamName));
        }

    }
}

