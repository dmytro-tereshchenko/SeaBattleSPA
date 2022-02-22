using AutoMapper;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.GameResources.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LimitSize, GameSizeLimit>()
                    .ForMember(dest => dest.FieldMinSizeX, opt => opt.MapFrom(c => c.MinSizeX))
                        .ForMember(dest => dest.FieldMinSizeY, opt => opt.MapFrom(c => c.MinSizeY))
                        .ForMember(dest => dest.FieldMaxSizeX, opt => opt.MapFrom(c => c.MaxSizeX))
                        .ForMember(dest => dest.FieldMaxSizeY, opt => opt.MapFrom(c => c.MaxSizeY));
        }
    }
}
