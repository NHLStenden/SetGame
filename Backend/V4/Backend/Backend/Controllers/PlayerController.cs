using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Models;
using Backend.Repository;
using Backend.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public virtual async Task<PlayerViewModel> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            
            var playerVm = _mapper.Map<PlayerViewModel>(entity);
            
            return playerVm;
        }
        
        [HttpGet]
        public async Task<IEnumerable<PlayerViewModel>> GetAsync()
        {
            var players = await _repository.GetAllAsync();

            var playerVms = _mapper.Map<IEnumerable<PlayerViewModel>>(players);
            
            return playerVms;
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerViewModel))]
        public async Task<IActionResult> CreateAsync(PlayerCreateViewModel playerCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var entity = _mapper.Map<Player>(playerCreateViewModel);
            
            var success = await _repository.AddAsync(entity);
            if (!success)
            {
                return BadRequest();
            }

            var createPlayerVm = _mapper.Map<PlayerViewModel>(entity);

            return CreatedAtAction("Get", new { id = entity.Id }, createPlayerVm);
        }
        
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerViewModel))]
        public async Task<ActionResult<PlayerViewModel>> UpdateAsync(int id, PlayerUpdateViewModel playerCreateViewModel)
        {
            if (id != playerCreateViewModel.Id)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = _mapper.Map<Player>(playerCreateViewModel);
            
            var success = await _repository.UpdateAsync(player);

            var playerViewModel = _mapper.Map<PlayerViewModel>(player);
            
            return success ? playerViewModel : NotFound();
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);

            return success ? 
                Ok() : 
                NotFound();
        }
    }
}