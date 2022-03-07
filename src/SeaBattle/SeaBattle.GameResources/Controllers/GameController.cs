using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaBattle.GameResources.Dto;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Repositories;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<GameSizeLimitDto> GetLimits([FromServices] IInitializeManager initializeService, [FromServices] IMapper mapper)
        {
            GameSizeLimitDto dto = mapper.Map<LimitSize, GameSizeLimitDto>(initializeService.GetLimitSizeField());

            dto.MaxPlayerSize = initializeService.GetMaxNumberOfPlayers();

            return dto;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDto>> GetById([FromServices] GenericRepository<Game> rep, [FromServices] IMapper mapper, int id)
        {
            IGame game = await rep.FindByIdAsync(id);

            if (game is null)
            {
                return NotFound();
            }

            GameDto dto = mapper.Map<IGame, GameDto>(game);

            return dto;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> Create([FromServices] IInitializeManager initializeService,
            [FromServices] IMapper mapper,
            [FromBody] GameCreateDto players)
        {
            IGame game = await initializeService.CreateGame(players.players);

            string name = HttpContext.User.FindFirst("name")?.Value;

            var response = await initializeService.AddPlayerToGame(game.Id, name);

            if (response.State == StateCode.Success)
            {
                GameDto dto = mapper.Map<IGame, GameDto>(response.Value);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }

            return BadRequest(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDto>> Get([FromServices] GenericRepository<Game> rep,
            [FromServices] IMapper mapper)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var query = await rep.GetWithIncludeAsync(g => g.GamePlayers);
            Game game = query.FirstOrDefault(g => g.GameState != GameState.Finished && g.GamePlayers.FirstOrDefault(g => g.Name.Equals(name)) is not null);

            if (game is null)
            {
                return NotFound();
            }

            GameDto dto = mapper.Map<Game, GameDto>(game);

            return dto;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameSearchDto[]>> GetSearch([FromServices] GenericRepository<Game> rep,
            [FromServices] IMapper mapper)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var query = await rep.GetWithIncludeAsync(g => g.GamePlayers, g => g.GameField);

            //take the current game for wait another player
            Game[] games = query.Where(g => g.GamePlayers.FirstOrDefault(g => g.Name.Equals(name)) is not null && (g.GameState == GameState.SearchPlayers || g.GameState == GameState.Init)).ToArray();

            //otherwise take array of games to join
            if (games is null || games.Length == 0)
            {
                games = query.Where(g => g.GamePlayers.FirstOrDefault(g => g.Name.Equals(name)) is null && g.GameState == GameState.SearchPlayers).ToArray();
            }

            if (games is null)
            {
                return NotFound();
            }

            GameSearchDto[] dto = mapper.Map<Game[], GameSearchDto[]>(games);

            return dto;
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GameDto>> JoinPlayer([FromServices] IInitializeManager initializeService,
            [FromServices] IMapper mapper,
            [FromBody] JoinPlayersDto player)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var response = await initializeService.AddPlayerToGame(player.gameId, name);

            if (response.State == StateCode.Success)
            {
                GameDto dto = mapper.Map<IGame, GameDto>(response.Value);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }

            return BadRequest(response);
        }
    }
}
