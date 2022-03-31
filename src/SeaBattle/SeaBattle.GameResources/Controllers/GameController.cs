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
            var query = await rep.GetWithIncludeAsync(g => g.Id == id, g => g.GamePlayers);

            IGame game = query.FirstOrDefault();

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GameDto>> JoinPlayer([FromServices] IInitializeManager initializeService,
            [FromServices] IMapper mapper,
            [FromBody] PlayerStateDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var response = await initializeService.AddPlayerToGame(data.gameId, name);

            if (response.State == StateCode.Success)
            {
                GameDto dto = mapper.Map<IGame, GameDto>(response.Value);

                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StateCode>> ReadyPlayer([FromServices] IInitializeManager initializeService,
            [FromBody] PlayerStateDto data)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var response = await initializeService.ReadyPlayer(data.gameId, name);

            if (response == StateCode.Success)
            {
                return response;
            }

            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StateCode>> EndMove([FromServices] IActionManager actionService, [FromServices] GenericRepository<Game> rep, int id)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var query = await rep.GetWithIncludeAsync(g => g.Id == id, g => g.GamePlayers);

            Game game = query.FirstOrDefault();

            if (game is null)
            {
                return BadRequest();
            }

            GamePlayer player = game.GamePlayers.FirstOrDefault(p => p.Name == name);

            if(player is null || player.Id != game.CurrentGamePlayerMoveId)
            {
                return BadRequest();
            }

            return await actionService.NextMove(id);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetWinner([FromServices] GenericRepository<Game> repGame, [FromServices] GenericRepository<GamePlayer> repPlayer, int id)
        {
            Game game = await repGame.FindByIdAsync(id);

            if(game is null || game.WinnerId is null)
            {
                return NotFound();
            }

            GamePlayer player = await repPlayer.FindByIdAsync(game.WinnerId ?? -1);

            if (player is null)
            {
                return NotFound();
            }

            return player.Name;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StateCode>> QuitGame([FromServices] IInitializeManager initializeService, [FromServices] GenericRepository<Game> rep, int id)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var query = await rep.GetWithIncludeAsync(g => g.Id == id, g => g.GamePlayers);

            Game game = query.FirstOrDefault();

            if (game is null)
            {
                return BadRequest();
            }

            GamePlayer player = game.GamePlayers.FirstOrDefault(p => p.Name == name);

            if (player is null || player.Id != game.CurrentGamePlayerMoveId)
            {
                return BadRequest();
            }

            return await initializeService.QuitGame(id, name);
        }
    }
}
