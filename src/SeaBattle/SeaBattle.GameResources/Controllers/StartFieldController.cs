using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Controllers
{
    public class StartFieldController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StartFieldDto>> Get([FromServices] IInitializeManager initializeService, [FromServices] IMapper mapper, int id)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var response = await initializeService.GetStartField(id, name);

            if (response.State != StateCode.Success)
            {
                return BadRequest(response.State.ToString());
            }

            IStartField startField = response.Value;

            StartFieldDto dto = mapper.Map<IStartField, StartFieldDto>(startField);

            return dto;
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> TransferShipFromGameField([FromServices] IActionManager actionService, RemoveShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.TransferShipFromGameField(name, data.ShipId, data.StartFieldId);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StateCode>> TransferShipToGameField([FromServices] IActionManager actionService, PutShipDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            return await actionService.TransferShipToGameField(name, data.TPosX, data.TPosY, (DirectionOfShipPosition)data.Direction, data.StartFieldId, data.ShipId);
        }
    }
}
