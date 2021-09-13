using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class CardController : CrudController<Card>
    {
        public CardController(ICardRepository repository) : base(repository)
        {
        }
    }
}