using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class CardController : CrudController<Card>
    {
        protected CardController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}