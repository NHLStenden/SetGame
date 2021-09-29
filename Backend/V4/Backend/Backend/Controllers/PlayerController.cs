using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync(PlayerCreateModel playerCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var entity = _mapper.Map<Player>(playerCreateModel);
            
            var success = await _repository.AddAsync(entity);
            if (!success)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<Player>> UpdateAsync(int id, PlayerUpdateModel playerCreateModel)
        {
            if (id != playerCreateModel.Id)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var player = _mapper.Map<Player>(playerCreateModel);
            
            var success = await _repository.UpdateAsync(player);

            return success ? player : NotFound();
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);

            return success ? 
                Ok() : 
                NotFound();
        }
    }
}