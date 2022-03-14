using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Controllers
{
    public class ShipController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameShipDto>> GetById([FromServices] GenericRepository<GameShip> rep, [FromServices] IMapper mapper, int id)
        {
            var query = await rep.GetWithIncludeAsync(s => s.Id == id,
                s => s.Ship,
                s => s.Weapons,
                s => s.Repairs);

            IGameShip gameShip = query.FirstOrDefault();

            if (gameShip is null)
            {
                return NotFound();
            }

            GameShipDto dto = mapper.Map<IGameShip, GameShipDto>(gameShip);

            return dto;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<ShipDto>>> GetShoppingList([FromServices] GenericRepository<Ship> rep, [FromServices] IMapper mapper)
        {
            ICollection<Ship> ships = await rep.GetAllAsync();

            ICollection<ShipDto> dto = mapper.Map<ICollection<Ship>, ICollection<ShipDto>>(ships);

            return dto.ToArray();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<WeaponDto>>> GetWeapons([FromServices] GenericRepository<Weapon> rep, [FromServices] IMapper mapper)
        {
            ICollection<Weapon> weapons = await rep.GetAllAsync();

            ICollection<WeaponDto> dto = mapper.Map<ICollection<Weapon>, ICollection<WeaponDto>>(weapons);

            return dto.ToArray();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<RepairDto>>> GetRepairs([FromServices] GenericRepository<Repair> rep, [FromServices] IMapper mapper)
        {
            ICollection<Repair> repairs = await rep.GetAllAsync();

            ICollection<RepairDto> dto = mapper.Map<ICollection<Repair>, ICollection<RepairDto>>(repairs);

            return dto.ToArray();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StateCode>> BuyShip([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] ShopShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            IGameShip ship = await shipService.GetNewShip(name, data.ShipId);

            StateCode state = await shipService.BuyShip(ship.Id, data.StartFieldId);

            return CreatedAtAction(nameof(GetById), new { id = ship.Id }, state);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> SellShip([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] ShopShipDto data)
        {
            return await shipService.SellShip(data.ShipId, data.StartFieldId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> AddWeapon([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] EquipmentDto data)
        {
            return await shipService.AddWeapon(data.ShipId, data.EquipmentId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> AddRepair([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] EquipmentDto data)
        {
            return await shipService.AddRepair(data.ShipId, data.EquipmentId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> RemoveWeapon([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] EquipmentDto data)
        {
            return await shipService.RemoveWeapon(data.ShipId, data.EquipmentId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> RemoveRepair([FromServices] IShipManager shipService, [FromServices] IMapper mapper, [FromBody] EquipmentDto data)
        {
            return await shipService.RemoveRepair(data.ShipId, data.EquipmentId);
        }
    }
}
