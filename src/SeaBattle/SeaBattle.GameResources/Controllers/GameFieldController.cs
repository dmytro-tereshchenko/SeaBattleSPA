﻿using AutoMapper;
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
        public async Task<ActionResult<GameFieldDto>> GetById([FromServices] GenericRepository<GameField> rep, [FromServices] IMapper mapper, int id)
        {
           var query = await rep.GetAsync(f => f.GameId == id);

            IGameField gameField = query.FirstOrDefault();

            if (gameField == null)
            {
                return NotFound();
            }

            GameFieldDto dto = mapper.Map<IGameField, GameFieldDto>(gameField);

            return dto;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GameFieldDto>> Create([FromServices] IInitializeManager initializeService, [FromServices] IMapper mapper, [FromBody] GameFieldCreateDto createData)
        {
            var response = await initializeService.CreateGameField(createData.GameId, createData.SizeX, createData.SizeY);

            if(response.State != StateCode.Success)
            {
                return BadRequest(response.State.ToString());
            }

            IGameField gameField = response.Value;

            GameFieldDto dto = mapper.Map<IGameField, GameFieldDto>(gameField);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDto>> Get([FromServices] GenericRepository<Game> rep, [FromServices] IMapper mapper)
        {
            string name = HttpContext.User.FindFirst("name")?.Value;

            var query = await rep.GetWithIncludeAsync(g => g.GamePlayers);
            Game game = query.FirstOrDefault(g => g.GamePlayers.FirstOrDefault(g => g.Name.Equals(name)) != null);

            if (game == null)
            {
                return NotFound();
            }

            GameDto dto = mapper.Map<Game, GameDto>(game);

            return dto;
        }*/
    }
}
