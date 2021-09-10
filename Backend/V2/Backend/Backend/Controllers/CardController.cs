using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class CardController : CrudController<Card>
    {
        public CardController(IGenericRepository<Card> repository) : base(repository)
        {
        }
    }
}