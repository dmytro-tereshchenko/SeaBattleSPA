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

            CreateMap<GameFieldCell, GameFieldCellDto>()
                .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(c => c.GameShip.GamePlayerId));

            CreateMap<StartField, StartFieldDto>()
                .ForMember(dest => dest.GameShipsId, opt => opt.MapFrom(c => c.GameShips));

            CreateMap<StartFieldCell, StartFieldCellDto>();

            CreateMap<GameShip, int>()
                .ConvertUsing(source => source.Id);

            CreateMap<Ship, ShipDto>()
                .ForMember(dest => dest.ShipType, opt => opt.MapFrom(c => (byte)c.ShipType));

            CreateMap<GameShip, GameShipDto>()
                .ForMember(dest => dest.ShipType, opt => opt.MapFrom(c => (byte)c.ShipType))
                .ForMember(dest => dest.Weapons, opt => opt.MapFrom(c => c.EquippedWeapons))
                .ForMember(dest => dest.Repairs, opt => opt.MapFrom(c => c.EquippedRepairs));

            CreateMap<Weapon, WeaponDto>();

            CreateMap<EquippedWeapon, WeaponDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(c => c.Weapon.Id))
                .ForMember(dest => dest.AttackRange, opt => opt.MapFrom(c => c.Weapon.AttackRange))
                .ForMember(dest => dest.Damage, opt => opt.MapFrom(c => c.Weapon.Damage));

            CreateMap<Repair, RepairDto>();

            CreateMap<EquippedRepair, RepairDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(c => c.Repair.Id))
                .ForMember(dest => dest.RepairRange, opt => opt.MapFrom(c => c.Repair.RepairRange))
                .ForMember(dest => dest.RepairPower, opt => opt.MapFrom(c => c.Repair.RepairPower));
        }
    }
}
