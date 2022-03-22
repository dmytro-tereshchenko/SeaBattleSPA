using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Repositories;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Controllers
{
    public class GameFieldController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameFieldDto>> GetById([FromServices] GenericRepository<GameField> repField,
            [FromServices] GenericRepository<GameShip> repShip,
            [FromServices] IMapper mapper, int id)
        {
            var query = await repField.GetWithIncludeAsync(f => f.GameId == id, f => f.GameFieldCells);

            IGameField gameField = query.FirstOrDefault();

            if (gameField is null)
            {
                return NotFound();
            }

            foreach (var cell in gameField.GameFieldCells)
            {
                await repShip.FindByIdAsync(cell.GameShipId);
            }

            GameFieldDto dto = mapper.Map<IGameField, GameFieldDto>(gameField);

            return dto;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameFieldDto>> Create([FromServices] IInitializeManager initializeService, [FromServices] IMapper mapper, [FromBody] GameFieldCreateDto createData)
        {
            var response = await initializeService.CreateGameField(createData.GameId, createData.SizeX, createData.SizeY);

            if(response.State != StateCode.Success)
            {
                return BadRequest(response.State.ToString());
            }

            IGameField gameField = response.Value;

            GameFieldDto dto = mapper.Map<IGameField, GameFieldDto>(gameField);

            return CreatedAtAction(nameof(GetById), new { id = dto.GameId }, dto);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> MoveShip([FromServices] IActionManager actionService, [FromBody] MoveShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.MoveShip(name, data.GameShipId, data.TPosX, data.TPosY, (DirectionOfShipPosition)data.Direction, data.GameFieldId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> AttackShip([FromServices] IActionManager actionService, [FromBody] AttackShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.AttackShip(name, data.GameShipId, data.TPosX, data.TPosY, data.GameFieldId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> RepairShip([FromServices] IActionManager actionService, [FromBody] RepairShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.RepairShip(name, data.GameShipId, data.TPosX, data.TPosY, data.GameFieldId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> RepairAllShip([FromServices] IActionManager actionService, [FromBody] RepairAllShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.RepairAllShip(name, data.GameShipId, data.GameFieldId);
        }
    }
}
