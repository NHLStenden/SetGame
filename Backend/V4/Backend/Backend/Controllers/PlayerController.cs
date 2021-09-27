using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : CrudController<Player>
    {
        public PlayerController(IPlayerRepository repository) : base(repository)
        { }
    }
}