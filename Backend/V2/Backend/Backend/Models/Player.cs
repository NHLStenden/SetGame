using Dapper.Contrib.Extensions;

namespace Backend.Models
{
    [Table("Player")]
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }
}