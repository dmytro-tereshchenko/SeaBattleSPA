using AutoMapper;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.GameResources.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LimitSize, GameSizeLimitDto>()
                    .ForMember(dest => dest.FieldMinSizeX, opt => opt.MapFrom(c => c.MinSizeX))
                        .ForMember(dest => dest.FieldMinSizeY, opt => opt.MapFrom(c => c.MinSizeY))
                        .ForMember(dest => dest.FieldMaxSizeX, opt => opt.MapFrom(c => c.MaxSizeX))
                        .ForMember(dest => dest.FieldMaxSizeY, opt => opt.MapFrom(c => c.MaxSizeY));

            CreateMap<Game, GameDto>()
                    .ForMember(dest => dest.CurrentPlayerMove, opt => opt.MapFrom(c => c.CurrentGamePlayerMoveId == null ? null : (c.GamePlayers as List<GamePlayer>).Find(p => p.Id == c.CurrentGamePlayerMoveId).Name))
                    .ForMember(dest => dest.GameState, opt => opt.MapFrom(c => (byte)c.GameState))
                    .ForMember(dest => dest.Players, opt => opt.MapFrom(c => c.GamePlayers));

            CreateMap<Game, GameSearchDto>()
                    .ForMember(dest => dest.GameFieldSize, opt => opt.MapFrom(c => $"{c.GameField.SizeX}x{c.GameField.SizeY}"))
                    .ForMember(dest => dest.GameState, opt => opt.MapFrom(c => (byte)c.GameState))
                    .ForMember(dest => dest.Players, opt => opt.MapFrom(c => string.Join(", ", c.GamePlayers.Select(p => p.Name).ToArray())));

            CreateMap<GamePlayer, PlayerDto>()
                    .ForMember(dest => dest.PlayerState, opt => opt.MapFrom(c => (byte)c.PlayerState));

            CreateMap<GameField, GameFieldDto>();

            CreateMap<GameFieldCell, GameFieldCellDto>();
        }
    }
}
