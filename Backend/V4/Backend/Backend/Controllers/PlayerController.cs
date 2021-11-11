using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerRepository playerRepository, IGameRepository gameRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public virtual async Task<PlayerDto> GetByIdAsync(int id)
        {
            var entity = await _playerRepository.GetByIdAsync(id);
            
            var playerDto = _mapper.Map<PlayerDto>(entity);
            
            return playerDto;
        }
        
        [HttpGet]
        public async Task<IEnumerable<PlayerDto>> GetAsync()
        {
            var players = await _playerRepository.GetAllAsync();

            var playerDtos = _mapper.Map<IEnumerable<PlayerDto>>(players);
            return playerDtos;
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerDto))]
        public async Task<IActionResult> CreateAsync(PlayerCreateDto playerCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var entity = _mapper.Map<Player>(playerCreateDto);
            
            var success = await _playerRepository.AddAsync(entity);
            if (!success)
            {
                return BadRequest();
            }

            var createPlayerDto = _mapper.Map<PlayerDto>(entity);

            return CreatedAtAction("Get", new { id = entity.Id }, createPlayerDto);
        }
        
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerDto))]
        public async Task<ActionResult<PlayerDto>> UpdateAsync(int id, PlayerUpdateDto playerCreateDto)
        {
            if (id != playerCreateDto.Id)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = _mapper.Map<Player>(playerCreateDto);
            
            var success = await _playerRepository.UpdateAsync(player);

            var playerDto = _mapper.Map<PlayerDto>(player);
            
            return success ? playerDto : NotFound();
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _playerRepository.DeleteAsync(id);

            return success ? 
                Ok() : 
                NotFound();
        }

        [HttpGet("{playerId:int}/Games")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames(int playerId)
        {
            var player = _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                return NotFound();
            
            var games = await _gameRepository.GetGamesForPlayer(playerId);
            var gameDtos = _mapper.Map<GameDto>(games);

            return Ok(gameDtos);
        }

        [HttpGet("{playerId:int}/Games/{gameId:int}")]
        public async Task<ActionResult<GameDto>> GetGame(int playerId, int gameId)
        {
            var player = _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                return NotFound();

            var game = await _gameRepository.GetByIdAsync(gameId);

            var gameDto = _mapper.Map<GameDto>(game);
            
            return Ok(gameDto);
        }
    }
}