using System.Collections.Generic;
using Backend.Models;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class PlayerController : CrudController<Player>
    {
        public PlayerController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}