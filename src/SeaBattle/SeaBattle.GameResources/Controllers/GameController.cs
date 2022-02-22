using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;

namespace SeaBattle.GameResources.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<GameSizeLimit> GetLimits([FromServices]IInitializeManager initializeService, [FromServices]IMapper mapper)
        {
            GameSizeLimit dto = mapper.Map<LimitSize, GameSizeLimit>(initializeService.GetLimitSizeField());

            dto.MaxPlayerSize = initializeService.GetMaxNumberOfPlayers();
            
            return dto;
        }
    }
}
